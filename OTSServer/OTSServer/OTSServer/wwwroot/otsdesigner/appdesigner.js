import { Component, Colors, deltaPos } from "./appblueprints.js";
var DragMode;
(function (DragMode) {
    DragMode[DragMode["None"] = 0] = "None";
    DragMode[DragMode["Left"] = 1] = "Left";
    DragMode[DragMode["Middle"] = 2] = "Middle";
})(DragMode || (DragMode = {}));
class Canvas {
    initCallbacks() {
        this.onSelectCallback = null;
    }
    initCanvas() {
        this.canvas = document.getElementById('design-canvas');
        this.context = this.canvas.getContext('2d');
        this.scaleCanvas();
    }
    initEventListeners() {
        this.onMousePress = this.onMousePress.bind(this);
        this.onMouseRelease = this.onMouseRelease.bind(this);
        this.onMouseMove = this.onMouseMove.bind(this);
        this.onMouseLost = this.onMouseLost.bind(this);
        this.onScaleChange = this.onScaleChange.bind(this);
        this.canvas.addEventListener("mousedown", this.onMousePress);
        this.canvas.addEventListener("mouseup", this.onMouseRelease);
        this.canvas.addEventListener("mousemove", this.onMouseMove);
        this.canvas.addEventListener("mouseout", this.onMouseLost);
        window.addEventListener('resize', this.onScaleChange);
    }
    initMisc() {
        this.mousePos = null;
        this.selected = null;
        this.mouseHold = DragMode.None;
        this.components = [];
        this.updateSpeed = 0.01;
        this.cycleCenter = this.getCanvasCenter();
        this.rotAngle = 0.0;
        this.rotSpeed = 2.5;
        this.rotFlip = false;
        this.canvasCenter = { x: 0, y: 0 };
    }
    update() {
        this.rotAngle += this.rotSpeed;
        if (this.rotAngle > 360) {
            this.rotAngle -= 360;
            this.rotFlip = !this.rotFlip;
        }
        this.redraw();
    }
    constructor() {
        console.log('Readying canvas....');
        this.initCanvas();
        this.initEventListeners();
        console.log(this.canvas == null ? 'Failed' : 'Success');
        this.initMisc();
        this.initCallbacks();
        setInterval(() => { this.update(); }, this.updateSpeed * 1000);
    }
    normalMousePos(e) {
        var rect = this.canvas.getBoundingClientRect();
        return {
            x: e.clientX - rect.left,
            y: e.clientY - rect.top
        };
    }
    onScaleChange() {
        console.log('SCALE CHANGE');
        this.scaleCanvas();
    }
    scaleCanvas() {
        this.canvas.style.width = "100%";
        this.canvas.style.height = "100%;";
        this.canvas.width = this.canvas.offsetWidth;
        this.canvas.height = this.canvas.offsetHeight;
    }
    clearCanvas() {
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }
    drawCycle() {
        let ctx = this.context;
        let center = this.getCanvasCenter();
        let angle = (this.rotAngle * Math.PI) / 180;
        ctx.strokeStyle = Colors.Green;
        this.context.beginPath();
        ctx.lineWidth = 10;
        this.rotAngle = Math.max(0, this.rotAngle);
        this.rotAngle = Math.min(360, this.rotAngle);
        if (this.rotFlip) {
            ctx.arc(center.x, center.y, 100, angle, 0, false);
        }
        else {
            ctx.arc(center.x, center.y, 100, 2 * Math.PI, angle, false);
        }
        ctx.stroke();
    }
    drawCrosshair() {
        let ctx = this.context;
        let cnv = this.canvas;
        let center = this.cycleCenter;
        ctx.lineWidth = 2;
        ctx.strokeStyle = Colors.Green;
        ctx.globalAlpha = .3;
        ctx.beginPath();
        ctx.moveTo(0, center.y);
        ctx.lineTo(cnv.width, center.y);
        ctx.moveTo(center.x, 0);
        ctx.lineTo(center.x, cnv.height);
        ctx.stroke();
        ctx.globalAlpha = 1;
    }
    redraw() {
        this.clearCanvas();
        let ctx = this.context;
        let pos = this.mousePos;
        //Draw all components
        this.components.forEach(e => { e.draw(this.context); });
        //this.drawCycle();
        this.drawCrosshair();
        this.drawMouseCircle();
    }
    objectAt(pos) {
        for (let i = this.components.length - 1; i >= 0; --i) {
            let cmp = this.components[i];
            if (cmp.inBound(pos)) {
                return cmp;
            }
        }
        return null;
    }
    moveToFront(obj) {
        if (obj == null) {
            return null;
        }
        let cmp = obj;
        var idx = this.components.indexOf(cmp);
        var rem = this.components.splice(idx, 1);
        this.components.push(rem[0]);
    }
    drawMouseCircle() {
        if (!this.mouseHold) {
            return false;
        }
        let pos = this.mousePos;
        if (pos == null) {
            return;
        }
        this.context.beginPath();
        this.context.lineWidth = 2;
        this.context.strokeStyle = 'red';
        this.context.arc(pos.x, pos.y, 20, 0, 2 * Math.PI);
        this.context.stroke();
    }
    translateCanvas(delta) {
        this.canvasCenter = {
            x: this.canvasCenter.x + delta.x,
            y: this.canvasCenter.y + delta.y
        };
        this.cycleCenter = {
            x: this.cycleCenter.x + delta.x,
            y: this.cycleCenter.y + delta.y
        };
        for (let i = 0; i < this.components.length; ++i) {
            this.components[i].drag(delta, this.context);
        }
        console.log(`Center: ${this.canvasCenter.x}, ${this.canvasCenter.y}`);
    }
    onMousePress(e) {
        if (e.button == 0) { //left
            this.mouseHold = DragMode.Left;
            var obj = this.objectAt(this.mousePos);
            if (obj !== null) {
                if (this.selected !== null && this.selected !== obj) {
                    this.selected.setUnselected();
                    console.log(`UNSELECT: ${this.selected.otsClass}`);
                }
                this.selected = obj;
                this.selected.setSelected();
                this.moveToFront(this.selected);
                console.log(`SELECT: ${obj.otsClass}`);
            }
            else if (this.selected !== null) {
                this.selected.setUnselected();
                this.selected = null;
            }
        }
        else if (e.button == 1) { //center
            this.mouseHold = DragMode.Middle;
        }
        else if (e.button == 2) { //right
        }
        this.redraw();
    }
    onMouseRelease(e) {
        this.mouseHold = DragMode.None;
        this.redraw();
    }
    onMouseMove(e) {
        let oldMouse = this.mousePos ?? this.normalMousePos(e);
        this.mousePos = this.normalMousePos(e);
        let selected = this.selected;
        let delta = deltaPos(oldMouse, this.mousePos);
        if (this.mouseHold === DragMode.Left) {
            if (selected !== null) {
                //drag
                if (selected instanceof Component) {
                    selected.drag(delta, this.context);
                }
                //DrawLink
                else if (selected instanceof Node) {
                    console.log('Selected node');
                    //TODO
                }
            }
        }
        else if (this.mouseHold === DragMode.Middle) {
            this.translateCanvas(delta);
        }
        if (this.selected != null) {
        }
        this.redraw();
    }
    onMouseLost(e) {
        this.mousePos = null;
    }
    getCanvasCenter() {
        let w = this.canvas.width / 2;
        let h = this.canvas.height / 2;
        return { x: w, y: h };
    }
    setOnObjectSelect(callback) {
        this.onSelectCallback = callback;
    }
    addComponent(component) {
        var center = this.getCanvasCenter();
        component.setPos(this.getCanvasCenter(), this.context);
        this.components.push(component);
        this.redraw();
    }
}
export class CanvasController {
    showAlert() { alert("Test alert"); }
    constructor() {
        this.viewAnchor = [0, 0];
        this.components = [];
        this.canvas = new Canvas();
    }
    addComponent(component) {
        this.canvas.addComponent(component);
    }
}
//# sourceMappingURL=appdesigner.js.map