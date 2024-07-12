import { } from "./appblueprints.js"
import { CanvasController } from "./appdesigner.js"
import { toOtsJObj } from "./otsclassconverter.js"

export class DesignManager {

    private cvController : CanvasController;

    constructor() {
        this.cvController = new CanvasController();
    }

    public insertComponent(compDef:string) {
        var comp = toOtsJObj(compDef);
        this.cvController.addComponent(comp);
    }

    public getApplicationMap() {
        alert('APP MAP')
        return "App Data";
    }

}

export function initDesigner() : DesignManager {
    return new DesignManager();
}
