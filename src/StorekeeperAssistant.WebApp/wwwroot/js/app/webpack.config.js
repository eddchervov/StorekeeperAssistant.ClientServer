const { VueLoaderPlugin } = require('vue-loader')
const path = require('path') 
const mode = process.env.mode

const config = {
    mode: mode,
    entry: {
        "moving-create": "./src/moving/create/index.js"
    },
    output: {
        path: path.resolve(__dirname, './build'),
        filename: '[name].js'
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [
                    'vue-style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            }
        ]
    },
    plugins: [new VueLoaderPlugin()]
}

if (mode === 'development') {
    config.devServer = {
        contentBase: [path.join(__dirname, 'public'), path.join(__dirname, 'build')],
        port: 9000,
        watchContentBase: true,
        writeToDisk: true
    }
    config.devtool = 'inline-source-map'
}

module.exports = config