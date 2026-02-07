const path = require('path');

module.exports = {
    mode: 'production',
    entry: './tsgen/azmaps/index.js',
    output: {
        filename: 'marqdouj-azuremaps-blazor.js',
        path: path.resolve(__dirname, 'wwwroot'),
        library: 'marqdoujAzureMapsBlazor',
    },
    externals: {
        "azure-maps-control": "atlas",
        "azure-maps-animations": "atlas"
    }
};