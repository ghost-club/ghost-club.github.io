## How to use ?

### Architecture

- Entry point of your application is `src/App.fs`
- We are using [hmtl-webpack-plugin](https://github.com/jantimon/html-webpack-plugin) to make `src/index.html` the entry point of the website
- Entry point of your style is `src/scss/main.scss`
    - [Bulma](https://bulma.io/) and [Font Awesome](https://fontawesome.com/) are already included
    - We are supporting both `scss` and `sass` (by default we use `scss`)
- Static assets (favicon, images, etc.) should be placed in the `static` folder

### In development mode

1. Run: `fake build -t Watch`
2. Go to [http://localhost:8080/](http://localhost:8080/)

In development mode, we activate:

- [Hot Module Replacement](https://fable-elmish.github.io/hmr/), modify your code and see the change on the fly

### Build for production

1. Run: `fake build`
2. All the files needed for deployment are under the `output` folder.

## License

Everything under `static/` is **not free** (not open-source, not creative commons, etc) without exception.

Everything under `src/` is **not free** (not open-source) unless there is an explicit license text in it.

Every other things present in the repository (build scripts, configuration files, or this README) is license under the [Unlicense](https://unlicense.org/).

(Anything not present in this repository but will be downloaded/generated by user action (running build scripts etc) will follow their original license. For example, everything under `output/` would be **not free**).