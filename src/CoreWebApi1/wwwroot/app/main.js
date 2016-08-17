"use strict";
// angular
var core_1 = require('@angular/core');
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
// app
var app_component_1 = require('./app.component');
//import {APP_ROUTER_PROVIDERS} from './app.routes';
//import {SERVICE_PROVIDERS} from './services/service-providers';
//import {MODEL_PROVIDERS} from './models/model-providers';
core_1.enableProdMode();
platform_browser_dynamic_1.bootstrap(app_component_1.AppComponent, [])
    .catch(function (err) { return console.error(err); });
//# sourceMappingURL=main.js.map