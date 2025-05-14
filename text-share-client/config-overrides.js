const MonacoWebpackPlugin = require('monaco-editor-webpack-plugin');

module.exports = function override(config) {
    config.plugins.push(
        new MonacoWebpackPlugin({
            languages: ['python', 'javascript', 'html', 'css'] // Укажите нужные языки
        })
    );

    return config;
};