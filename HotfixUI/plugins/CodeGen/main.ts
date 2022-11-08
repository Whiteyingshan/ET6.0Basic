//FYI: https://github.com/Tencent/puerts/blob/master/doc/unity/manual.md

import { FairyEditor } from 'csharp';
import { genCode } from './GenCode';

function onPublish(handler: FairyEditor.PublishHandler) {
    //prevent default output
    handler.genCode = false;

    console.log('Handling gen code in plugin');
    //do it myself
    genCode(handler);
}

function onDestroy() {
    //do cleanup here
}

export { onPublish, onDestroy };