import { CanvasController } from "./appdesigner.js";
import { toOtsJObj } from "./otsclassconverter.js";
export class DesignManager {
    constructor() {
        this.cvController = new CanvasController();
    }
    insertComponent(compDef) {
        console.log(compDef);
        var comp = toOtsJObj(compDef);
        this.cvController.addComponent(comp);
    }
    getApplicationMap() {
        alert('APP MAP');
        return "App Data";
    }
}
export function initDesigner() {
    return new DesignManager();
}
//# sourceMappingURL=OTSDesignerManager.js.map