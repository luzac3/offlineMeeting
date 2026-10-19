const path = require('path');
const glob = require('glob');
const webpack = require('webpack');

let entries = {};
glob
    .sync("./appData/ts/**/Index.ts", {
        ignore: "./appData/ts/**/*.d.ts"
    }).
    map(function (file) {
        const key = file.replace("./appData/ts/", "").replace(/\.ts$/, "").split("/")[0];
        entries[key] = file;
    });

module.exports = (env, argv) => {
    const IS_DEVELOPMENT = argv.mode === "development";

    const configs = {

        devServer: {
            index: "Index.html",
            port: 9223,
        },

        entry: entries,

        output: {
            path: path.join(__dirname, "./"),
            filename: '[name].js'
        },

        module: {
            rules: [
                {
                    test: /\.ts$/,
                    use: 'ts-loader',
                }
            ],
        },
        resolve: {
            extensions: [
                '.ts', '.js',
            ],
            alias: {
                '@root': path.resolve(__dirname, './appData/ts/')
            }
        },
        target: 'electron-renderer'
    }

    if (IS_DEVELOPMENT) {
        configs.devtool = 'source-map';
    }

    return configs;
};
