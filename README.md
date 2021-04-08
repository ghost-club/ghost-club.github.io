# Fulma minimal template

This template setup a minimal application using [Fable](http://fable.io/), [Elmish](https://fable-elmish.github.io/) and [Fulma](https://mangelmaxime.github.io/Fulma/).

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
2. All the files needed for deployment are under the `docs` folder.
