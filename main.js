window.onload = _=>{

const startTime = new Date();
let curTime = new Date();
const resizeCallback = [];
function resize() {
  const nw = container.clientWidth;
  const nh = container.clientHeight;
  resizeCallback.forEach(cb=>{ cb(nw,nh); });
}
window.addEventListener("resize",resize);

const canvas = document.getElementById("canvas");
const container = document.getElementById("container");
const gl = canvas.getContext("webgl");
gl.getExtension("OES_texture_float");
gl.clearColor(0,0,0,1);
resizeCallback.push((w,h)=>{
  canvas.width = w;
  canvas.height = h;
  gl.viewport(0,0,w,h);
});

// Mesh and Material

function buildMesh(verts, dim) {
  const vbo = gl.createBuffer();
  gl.bindBuffer(gl.ARRAY_BUFFER,vbo);
  gl.bufferData(gl.ARRAY_BUFFER,new Float32Array(verts),gl.STATIC_DRAW);
  return _=>{
    gl.bindBuffer(gl.ARRAY_BUFFER,vbo);
    const stride = 4*dim;
    gl.vertexAttribPointer(0,dim,gl.FLOAT,false,stride,0);
    gl.enableVertexAttribArray(0);
    gl.drawArrays(gl.TRIANGLE_STRIP,0,verts.length/dim);
  };
}
const rect = buildMesh([-1,-1,1,-1,-1,1,1,1], 2);

function buildMaterial(vs,fs,mesh,params) {
  const prec = "precision mediump float;\n";
  function buildShader(type,src) {
    const s = gl.createShader(type);
    gl.shaderSource(s,prec+src);
    gl.compileShader(s);
    if(!gl.getShaderParameter(s,gl.COMPILE_STATUS)){
      console.error(gl.getShaderInfoLog(s));
      return null;
    }
    return s;
  }
  function buildProgram(v,f) {
    const p = gl.createProgram();
    gl.attachShader(p,v);
    gl.attachShader(p,f);
    gl.linkProgram(p);
    if(!gl.getProgramParameter(p,gl.LINK_STATUS)){
      console.error(gl.getProgramInfoLog(p));
    }
    return p;
  }
  const v = buildShader(gl.VERTEX_SHADER,vs);
  const f = buildShader(gl.FRAGMENT_SHADER,fs);
  const p = buildProgram(v,f);
  const locs = {};
  params.push("resolution");
  params.push("time");
  params.forEach(pa=>{
    locs[pa] = gl.getUniformLocation(p,pa);
  });
  function setting(name, args) {
    if(args.length == 1 && args[0].texture !== undefined) {
      gl.uniform1i(locs[name], args[0].texture);
    } else {
      const func = "uniform" + args.length + "fv";
      const as = [];
      for(let i=0;i<args.length;i++)as.push(args[i]);
      gl[func](locs[name],as);
    }
  }
  let task = [];
  const o = _=>{
    gl.useProgram(p);
    task.forEach(t=>{t();});
    task = [];
    setting("resolution",[canvas.width,canvas.height]);
    setting("time", [(curTime - startTime) / 1000]);
    gl.bindAttribLocation(p,0,"vertex");
    gl.bindAttribLocation(p,1,"normal");
    mesh();
  };
  params.forEach(l=>{
    o[l] = function() {
      task.push(_=>{setting(l, arguments);});
    };
  });
  return o;
}

const ModelTexture = img=>{
  const textureIndex = 0;
  let w = 16, h = 96;
  const tex = gl.createTexture();
  gl.bindTexture(gl.TEXTURE_2D, tex);
  gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, w, h, 0, gl.RGBA, gl.FLOAT, img);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.NEAREST);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.NEAREST);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);

  return {
    use: _=>{
      gl.activeTexture(gl.TEXTURE0 + textureIndex);
      gl.bindTexture(gl.TEXTURE_2D, tex);
      return { texture: textureIndex };
    }
  };
};

// Main Program
resize();

const present = buildMaterial(`
attribute vec2 vertex;
varying vec2 coord;
void main() {
    coord = vertex;
    gl_Position = vec4(vertex,0,1);
}
`,`
varying vec2 coord;
uniform sampler2D model;
uniform float time;
uniform vec2 resolution;
uniform vec3 accel;

#define ratio 0.4

vec3 unpack(ivec2 iuv) {
  iuv.y = 95 - iuv.y;
  return texture2D(model, (vec2(iuv)+0.5)/vec2(16,96)).xyz;
}

vec4 powers(float t) {
    return vec4(1, t, t*t, t*t*t);
}

vec2 flow(vec2 uv, ivec2 offset, ivec2 scale) {
    uv /= vec2(ratio, -1);
    uv = uv*0.5 + 0.5;
    vec2 coord = uv*vec2(scale-1);
    ivec2 i = ivec2(coord);
    vec2 f = coord - vec2(i);
    mat4 m = mat4(
        0, 2, 0, 0,
        -1, 0, 1, 0,
        2, -5, 4, -1,
        -1, 3, -3, 1
    ) / 2.;
    vec4 mx = m * powers(f.x);
    vec4 my = m * powers(f.y);
    vec2 s = vec2(0);
    // TODO: reduce costs
    for(int j=0;j<4;j++) {
        for(int k=0;k<4;k++) {
            ivec2 u = i + ivec2(j-1,k-1);
            if(u.x >= 0 && u.y >= 0 && u.x < scale.x && u.y < scale.y) {
                s += unpack(u + offset).xy * mx[j] * my[k];
            }
        }
    }
    return s;
}

vec2 flow3(vec2 x) {
    return flow(x, ivec2(0,0), ivec2(16,64));
}
vec2 flow2(vec2 x) {
    return flow(x, ivec2(0,64), ivec2(8,32));
}
vec2 flow1(vec2 x) {
    return flow(x, ivec2(8,64), ivec2(8,32));
}

float activation(vec2 x) {
    vec2 v = vec2(-24.063580, 23.359764);
    float b = 8.072939;
    float r = dot(x,v) + b;
    return r * 0.04;
}

float smax(float a, float b) {
    float k = 0.004;
    float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
    return mix( a, b, h ) + k*h*(1.0-h);
}

float eye(vec2 p) {
    p.x += 0.0254;
    p.x = abs(p.x);
    p.y += 0.6003;
    vec2 mp = p;
    float r = 0.03, mr = r*10.75/11.;
    float m = length(mp)-mr;
    m = smax(m, - mp.y - 0.0025);
    p.x -= 0.107;
    float e = length(p)-r;
    float eps = 0.005;
    return smoothstep(-eps, eps, min(m,e));
}

vec3 logoSample(vec2 uv) {
    vec2 p = uv*2.-1.;
    p *= vec2(ratio, -1);
    float e = eye(p);

    vec2 x = p;
    x += flow3(x) / 4.;
    x += flow2(x) / 2.;
    x += flow3(x) / 2.;
    x += flow2(x) / 1.;
    x = flow1(-x);
    float v = activation(x);
    v = mix(v, -v, e);
    return vec3(v + 0.5);
}

void main() {
    vec2 uv = coord * vec2(resolution.x/resolution.y,1) * vec2(1,ratio) * 1.25;
    vec3 col = logoSample(uv + 0.5);
    gl_FragColor = vec4(col,1);
}
`,rect,["model","accel"]);

let model = null;
let accelX = 0, accelY = 0, accelZ = 0;
/* 
if(window.DeviceMotionEvent && DeviceMotionEvent.requestPermission) {
  container.addEventListener("click",_=>{
    DeviceMotionEvent.requestPermission().then(_=>{
      window.addEventListener("devicemotion", e=>{
        accelX = e.accelerationIncludingGravity.x;
        accelY = e.accelerationIncludingGravity.y;
        accelZ = e.accelerationIncludingGravity.z;
      });
    });
  });
} else {
  window.addEventListener("devicemotion", e=>{
    accelX = e.accelerationIncludingGravity.x;
    accelY = e.accelerationIncludingGravity.y;
    accelZ = e.accelerationIncludingGravity.z;
  });
}
*/
function render() {
  curTime = new Date();
  if(model) present.model(model.use());
  present.accel(accelX, - accelY, accelZ);
  present();
  requestAnimationFrame(render);
}
render();

async function load() {
  const res = await fetch("/model.png");
  const blob = await res.blob();
  const img = new Image();
  img.onload = _=>{
    const cvs = document.createElement("canvas");
    const ctx = cvs.getContext("2d");
    cvs.width = img.width;
    cvs.height = img.height;
    ctx.drawImage(img, 0, 0);
    const data = ctx.getImageData(0, 0, img.width, img.height).data;
    const mem = new Float32Array(16*96*4);
    const buf = new ArrayBuffer(4);
    const u = new DataView(buf);
    for(let j=0;j<96;j++) {
      for(let i=0;i<16;i++) {
        for(let k=0;k<3;k++) {
          u.setUint8(0, data[(j*64+i*4)*4+0+k]);
          u.setUint8(1, data[(j*64+i*4)*4+4+k]);
          u.setUint8(2, data[(j*64+i*4)*4+8+k]);
          u.setUint8(3, data[(j*64+i*4)*4+12+k]);
          mem[(j*16+i)*4+k] = new DataView(buf).getFloat32(0);
        }
        mem[(j*16+i)*4+3] = 1.0;
      }
    }
    model = ModelTexture(mem);
  };
  img.src = URL.createObjectURL(blob);
}
load();

};
