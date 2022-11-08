"use strict";
//FYI: https://github.com/Tencent/puerts/blob/master/doc/unity/manual.md
Object.defineProperty(exports, "__esModule", { value: true });
exports.onDestroy = exports.onPublish = void 0;
const GenCode_1 = require("./GenCode");
function onPublish(handler) {
    //prevent default output
    handler.genCode = false;
    console.log('Handling gen code in plugin');
    //do it myself
    (0, GenCode_1.genCode)(handler);
}
exports.onPublish = onPublish;
function onDestroy() {
    //do cleanup here
}
exports.onDestroy = onDestroy;
