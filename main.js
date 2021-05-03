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
document.addEventListener("touchstart", function(e){
  if (e.touches.length > 1) {
    e.preventDefault();
  }
}, true);

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
    const t = (curTime - startTime) / 1000;
    const pr = Math.PI * 2 * 4;
    setting("time", [(t + pr/2) % pr - pr/2]);
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

const ImageTexture = img=>{
  const tex = gl.createTexture();
  gl.bindTexture(gl.TEXTURE_2D, tex);
  gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, img);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
  gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);

  return {
    use: _=>{
      gl.activeTexture(gl.TEXTURE0);
      gl.bindTexture(gl.TEXTURE_2D, tex);
      return { texture: 0 };
    }
  };
};

const ModelTexture = img=>{
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
      gl.activeTexture(gl.TEXTURE1);
      gl.bindTexture(gl.TEXTURE_2D, tex);
      return { texture: 1 };
    }
  };
};

resize();

const bgs = {
  "1": {
    scaling: true, eyeDist: false,
    code: `
      vec2 x = p;
      x += flow3(x) / 4.;
      x += flow2(x) / 2.;
      x += flow3(x) / 2.;
      x += flow2(x) / 1.;
      x.x = tanh(x.x*3.)/3.;
      x = mix(flow1(x), flow1(-x), cos(time*1.+p.y*16.)*0.5+0.5);
      float v = activation(x);
      vec3 col = sampleBg(p+x*vec2(sin(time+p.y*4.),0)*0.5);
      return mix(vec3(1.), mix(vec3(0.), col, smoothstep(10.,-0.05,v)), e);`
  },
  "2": {
    scaling: false, eyeDist: true,
    code: `
      vec2 x = p;
      float u = sin(time + p.x*1. + p.y*cos(roundTime.x*2.+p.x*3.))*4.;
      x.x = sin(x.x*u)/u;
      x += flow3(x) / 4. * (sin(time*1.+p.x*5.)*0.5+0.5);
      x += flow2(x) / 2. * (sin(time*2.+p.y*5.)*0.5+0.5);
      x += flow3(x) / 2. * (cos(time*0.5)*0.5+0.5);
      x += flow2(x) / 1. * (cos(time*1.)*0.5+0.5);
      x = flow1(x);
      float a = 2.5 + sin(time-p.x*5.)*0.2 + 3.1415926535;
      x *= mat2(cos(a),-sin(a),sin(a),cos(a));
      x += smoothstep(0.015,-0.005,e)*vec2(3.,-4.);
      vec3 col = sampleBg(p*0.2+x*0.3+vec2(0.3,-0.4));
      return col;`
  },
  "3": {
    scaling: false, eyeDist: false,
    code: `
      vec2 x = p;
      x += flow3(x) / 4. * (cos(time)+1.);
      x += flow2(x) / 2. * (cos(time*0.5+roundTime.x*2.)+1.);
      x += flow3(x) / 2.;
      x += flow2(x) / 1. * (sin(time)+1.);
      x = flow1(x);
      vec3 col = sampleBg(vec2(x.y*0.1+sin(time+p.x*p.y*2.)*0.05,sin(p.x*5.)));
      return mix(vec3(0.1,0,0), col, e);`
  },
  "5": {
    scaling: false, eyeDist: false,
    code: `
      vec2 x = p;
      float r = cos(time+0.5);
      x += flow3(x) / 4. * r;
      x += flow2(x) / 2. * r;
      x += flow3(x) / 2. * r;
      x += flow2(x) / 1. * r;
      x.x += time*0.5/3.1415926535 + cos(time+1.8)*0.2;
      x = mix(flow1(-vec2(fract(x.x)-0.5,x.y)), flow1(vec2(fract(x.x+0.5)-0.5,x.y)), tanh(cos(x.x*3.1415926535*2.)*5.)*0.5+0.5);
      x *= abs(p.x);
      float v = activation(x);
      v -= pow(abs(p.x),2.) * 10.;
      vec3 col = sampleBg(p.x*0.5+x+vec2(sin(time+1.5),cos(time+1.5)));
      col *= smoothstep(-0.01,0.01,v);
      return mix(vec3(0.9,1.0,1.0), col, e);`
  },
  "306": {
    scaling: false, eyeDist: true,
    code: `
      float eh = 1. - pow(sin(time*2. - p.y*4.)*0.5+0.5, 10.0);
      vec2 x = p;
      x += flow3(x) / 4. * mix(1., pow(sin(time*0.5 + x.y*9.)*0.5+0.5, 0.8), eh);
      x += flow2(x) / 2.;
      x += flow3(x) / 2. * mix(1., pow(sin(time*2. + dot(x,vec2(9,-7)))*0.5+0.5, 2.0), eh);
      x += flow2(x) / 1.;
      float rt = roundTime.x + p.y*0.04;
      float a = roundTime.y * sin(rt) * 2.;
      vec2 rx = mat2(cos(a),-sin(a),sin(a),cos(a)) * x;
      x = mix(flow1(x), flow1(rx), pow(cos(rt)*0.5+0.5,30.));
      x += vec2(-0.4,0.4) * eh;
      vec3 col = sampleBg(x*0.4 + uv*0.05);
      return mix(vec3(1.), col, smoothstep(-0.002,0.002,e));`
  },
  "nm": {
    scaling: false, eyeDist: false,
    code: `
      vec2 x = p;
      x += flow3(x) / 4. * (cos(time*0.5)*(p.y+1.)*1.+1.);
      x += flow2(x) / 2.;
      x += flow3(x) / 2. * (sin(time*0.5)*(p.y-1.)*1.+1.);
      x += flow2(x) / 1.;
      x = flow1(x);
      x.x += sin(time*0.5+p.y*5.)*1.;
      float v = activation(x);
      x = mix(x, p, 1.-exp(-max(0.,dot(p,p*vec2(1,0.2))-0.3))*0.4);
      vec2 u = p*0.5 + x*0.5;
      u = floor(u*5.)/5.;
      vec3 col = sampleBg(u + vec2(1.0,0)) * 2.;
      col += sampleBg(x + vec2(1.0,0)) * 2.;
      col *= smoothstep(-0.01,0.01,v);
      return mix(vec3(0.7,0.9,1)-col, col, e);`
  },
  "tv": {
    scaling: true, eyeDist: true,
    code: `
      float eh = 1. - pow(sin(time*2.)*0.5+0.5, 20.0) - pow(-sin(time*2.)*0.5+0.5, 10.0);
      vec2 x = p;
      x += flow3(x) / 4. * mix(1., cos(time*2.+p.x*10.)*2.+2., eh);
      x += flow2(x) / 2. * mix(1., sin(time*2.+p.y*4.)+1., eh);
      x += flow3(x) / 2. * mix(1., 0., eh);
      x += flow2(x) / 1. * mix(1., 0., eh);
      vec3 col1 = sampleBg(x);
      float a = time * 0.5;
      vec3 col2 = sampleBg(mat2(cos(a),-sin(a),sin(a),cos(a))*p) * 2.;
      x = flow1(x);
      float v = activation(x);
      v *= smoothstep(-0.05,0.05,e)*2.-1.;
      return mix(col1, col2, smoothstep(0.05,-0.05,v)) * max(smoothstep(-0.1,0.1,dot(p,p*vec2(1,0.2))-0.3), 1.-exp(-abs(v)*0.1));`
  },
  "u": {
    scaling: false, eyeDist: false,
    code: `
      vec2 x = p;
      float eh = abs(sin(roundTime.x*2.+roundTime.y*4.)) > 0.99 ? 0.0 : 1.0;
      float a = 1.0;
      x += flow3(x) / 4. * (sin(time*0.5)*0.5+1.0) * eh;
      x += flow2(x) / 2. * eh * (1. + pow(abs(sin(roundTime.x*2.+roundTime.y*4.)), 40.)*8.);
      x += flow3(x) / 2. * (cos(time*1.0)*0.5+1.0) * eh;
      x += flow2(x) / 1. * eh;
      vec2 u = x;
      u /= vec2(ratio, -1);
      u = u*0.5 + 0.5;
      vec2 coord = u*vec2(ivec2(8,32)-1);
      coord = floor(coord) + 0.5;
      coord += p.x;
      coord /= vec2(ivec2(8,32)-1);
      coord = coord*2.-1.;
      coord *= vec2(ratio, -1);
      x = flow1(coord);
      vec3 col = sampleBg(x*0.2 + vec2(cos(time),sin(time)) * 0.5);
      return mix(vec3(1.), col, e);`
  },
  "s": {
    scaling: false, eyeDist: false,
    code: `
      vec2 x = p;
      x += flow3(x) / 4.;
      x += flow2(x) / 2.;
      x += flow3(x) / 2.;
      x += flow2(x) / 1.;
      p = x + vec2(0,0.6);
      float t = atan(p.x,p.y) / 3.1415926535 / 2.;
      t += cos(length(p)*1.) * 0.3;
      t = abs(fract(t*5.)-0.5)/5.;
      t *= 3.1415926535 * 2.;
      p = vec2(cos(t),sin(t)) * length(p);
      p.x += time * 1. / 3.1415926535;
      p.y += time * 2. / 3.1415926535;
      p += activation(flow1(x)) * 0.001;
      vec3 col = sampleBg(fract(p));
      return mix(vec3(1,1,1), col, e);`
  },
  "3v": null /* {
    scaling: false, eyeDist: false,
    code: `
      p.x += 0.022;
      p.y += 0.6;
      p = vec2(p.x+p.y,p.x-p.y);
      p = vec2(p.x+p.y,p.x-p.y);
      vec2 x = p * 0.5;
      x += flow3(x) / 4.;
      x += flow2(x) / 2.;
      x += flow3(x) / 2.;
      x += flow2(x) / 1.;
      float t = atan(p.x,p.y) / 3.1415926535 / 2.;
      t = abs(fract(t*4.)-0.5)/4.;
      t *= 3.1415926535 * 2.;
      vec2 o = vec2(cos(t),sin(t)) * length(p) * 0.5;
      o.x += floor(time*2.)+pow(fract(time*2.),4.) + time;
      o.y += time / 3.1415926535;
      vec3 col = sampleBg(fract(o));
      return mix(1.-col, col, e);`
  } */,
  "m": null,
  "r": null,
  "u2": null,
  "u4": null
};
let ix = localStorage.getItem("index") | 0;
const bgBases = ["1","2","3","5","306","nm","u","s"];
const bgImageName = bgBases[Math.floor(Math.random()*bgBases.length)];
let seed = Math.floor((startTime / 1000 / 60 - startTime.getTimezoneOffset()) / 60 / 24) | 0;
const xorShift = _=>{
  seed = seed ^ (seed << 13);
  seed = seed ^ (seed >> 17);
  seed = seed ^ (seed << 5);
  return seed;
};
for(let i=0;i<2;i++) xorShift();
const bgNames = [];
// for(let i=0;i<3;i++) {
//   const l = bgBases.length;
//   let k = xorShift() % l;
//   if(k < 0) k += l;
//   bgNames.push(bgBases[k]);
//   bgBases.splice(k,1);
// }
// ix %= bgNames.length;
localStorage.setItem("index", ix+(Math.random() < 0.5 ? 1 : 2));
ix = Math.floor(Math.random()*bgBases.length);
const bgName = Math.random() < 0.01 ? "tv" : bgBases[ix];
const bgData = bgs[bgName];

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
uniform sampler2D bg;
uniform float time;
uniform vec2 roundTime;
uniform vec2 resolution;

#define ratio 0.4

vec3 pick(ivec2 iuv) {
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
    ivec2 i = ivec2(floor(coord));
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
    for(int j=0;j<4;j++) {
        for(int k=0;k<4;k++) {
            ivec2 u = i + ivec2(j-1,k-1);
            u.x = u.x < 0 ? 0 : u.x;
            u.x = u.x >= scale.x ? scale.x - 1 : u.x;
            u.y = u.y < 0 ? 0 : u.y;
            u.y = u.y >= scale.y ? scale.y - 1 : u.y;
            s += pick(u + offset).xy * mx[j] * my[k];
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
    vec2 v = vec2(-10.846223, 11.471219);
    float b = 8.681054;
    float r = dot(x,v) + b;
    return r;
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
    e = min(m,e);
    float eps = 0.002;
    return ${bgData.eyeDist ? "e" : "smoothstep(-eps, eps, e)"};
}

float tanh(float v) {
  return (exp(v) - exp(-v)) / (exp(v) + exp(-v));
}

vec3 sampleBg(vec2 p) {
    p *= vec2(0.9/1.6,1.0);
    float r = resolution.x/resolution.y*0.9/1.6;
    ${bgData.scaling ? "if(r > 1.0) p /= r;" : ""};
    return texture2D(bg, p*0.5+0.5).xyz;
}

vec3 logoSample(vec2 uv) {
    vec2 p = uv*2.-1.;
    p *= vec2(ratio, -1);
    float e = eye(p);
    ${bgData.code}
}

void main() {
    vec2 uv = coord * vec2(resolution.x/resolution.y,1) * vec2(1,ratio) * 1.25;
    vec3 col = logoSample(uv + 0.5);
    gl_FragColor = vec4(col,1);
}
`,rect,["model","bg","roundTime"]);

let model = null, bg = null;
let lastIx = -1, lastRandom = 0;
function render() {
  curTime = new Date();
  if(model && bg) {
    present.model(model.use());
    present.bg(bg.use());
    const t = (curTime - startTime) / 1000 / 20;
    const i = Math.floor(t);
    if(i != lastIx) lastRandom = Math.random() - 0.5, lastIx = i;
    present.roundTime((t-i-0.5)*3.1415926535, lastRandom);
    present();
  }
  requestAnimationFrame(render);
}
render();

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
img.src = "/model.png";
const bgImg = new Image();
bgImg.onload = _=>{
  bg = ImageTexture(bgImg);
};
bgImg.src = "/background/" + bgName + ".png";

};
