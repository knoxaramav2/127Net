import { Colors } from "./appblueprints.js";
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
        this.mouseHold = false;
        this.components = [];
        this.updateSpeed = 0.01;
        this.cycleCenter = { x: 0, y: 0 };
        this.rotAngle = 0.0;
        this.rotSpeed = 2.5;
        this.rotFlip = false;
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
        let center = this.canvasCenter();
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
        let center = this.canvasCenter();
        ctx.lineWidth = 2;
        ctx.strokeStyle = Colors.Green;
        ctx.beginPath();
        ctx.moveTo(0, center.y);
        ctx.lineTo(cnv.width, center.y);
        ctx.moveTo(center.x, 0);
        ctx.lineTo(center.x, cnv.height);
        ctx.stroke();
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
        return null;
    }
    moveToFront(obj) {
        if (obj == null) {
            return null;
        }
        var idx = this.components.indexOf(obj);
        var rem = this.components.splice(idx, 1);
        this.components.push(rem[0]);
    }
    drawMouseCircle() {
        if (!this.mouseHold) {
            return false;
        }
        let pos = this.mousePos;
        this.context.beginPath();
        this.context.lineWidth = 2;
        this.context.strokeStyle = 'red';
        this.context.arc(pos.x, pos.y, 20, 0, 2 * Math.PI);
        this.context.stroke();
    }
    onMousePress(e) {
        if (e.button == 0) { //left
            this.mouseHold = true;
            var obj = this.objectAt(this.mousePos);
        }
        else if (e.button == 1) { //center
        }
        else if (e.button == 2) { //right
        }
        this.redraw();
    }
    onMouseRelease(e) {
        this.mouseHold = false;
        this.redraw();
    }
    onMouseMove(e) {
        this.mousePos = this.normalMousePos(e);
        if (this.mouseHold) {
        }
        if (this.selected != null) {
        }
        this.redraw();
    }
    onMouseLost(e) {
        this.mousePos = null;
    }
    canvasCenter() {
        let w = this.canvas.width / 2;
        let h = this.canvas.height / 2;
        return { x: w, y: h };
    }
    setOnObjectSelect(callback) {
        this.onSelectCallback = callback;
    }
    addComponent(component) {
        var center = this.canvasCenter();
        component.setPos(this.canvasCenter());
        console.log(`CMP: ${center.x} -> ${component.pos.x}`);
        this.components.push(component);
        this.redraw();
        console.log(`Added component: ${component.name} | ${this.components.length}`);
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