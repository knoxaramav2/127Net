import { Component, Field, OTSObject, Dim, Colors, Shapes } from "./appblueprints.js";

class Canvas {
    canvas: HTMLCanvasElement;
    context: CanvasRenderingContext2D;

    selected: Component;
    components: Array<Component>;

    //Callbacks
    onSelectCallback: (component:Component) => void;
    onEditFieldSelectCallback: (field:Field) => void;

    //state
    mouseHold: boolean;
    mousePos: {x:number, y:number};

    updateSpeed: number;

    cycleCenter: Dim;
    rotAngle:number;
    rotSpeed:number;
    rotFlip: boolean;

    private initCallbacks(): void {
        this.onSelectCallback = null;
    }

    private initCanvas() {
        this.canvas = document.getElementById('design-canvas') as HTMLCanvasElement;
        this.context = this.canvas.getContext('2d');
        this.scaleCanvas();
    }

    private initEventListeners() {
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

    private initMisc() {
        this.mousePos = null;
        this.selected = null;
        this.mouseHold = false;
        this.components = [];

        this.updateSpeed = 0.01;

        this.cycleCenter = { x: 0, y: 0 }
        this.rotAngle = 0.0;
        this.rotSpeed = 2.5;
        this.rotFlip = false;
    }

    private update() {
        this.rotAngle += this.rotSpeed;
        if (this.rotAngle > 360) {
            this.rotAngle -= 360;
            this.rotFlip = !this.rotFlip;
        }
        
        this.redraw();
    }

    constructor() {
            
        console.log('Readying canvas....')
        this.initCanvas();
        this.initEventListeners();
        console.log(this.canvas == null ? 'Failed' : 'Success');
        this.initMisc();
        this.initCallbacks();
        setInterval(() => {this.update()}, this.updateSpeed*1000);
    }

    private normalMousePos(e: MouseEvent) {
        var rect = this.canvas.getBoundingClientRect();
        return {
            x: e.clientX - rect.left,
            y: e.clientY - rect.top
        }
    }

    private onScaleChange() {
        console.log('SCALE CHANGE');
        this.scaleCanvas();
    }

    private scaleCanvas() {
        this.canvas.style.width = "100%";
        this.canvas.style.height = "100%;";
        this.canvas.width = this.canvas.offsetWidth;
        this.canvas.height = this.canvas.offsetHeight;
    }

    private clearCanvas() {
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height)
    }

    private drawCycle() {
        
        let ctx = this.context;
        let center = this.canvasCenter();
        let angle = (this.rotAngle*Math.PI)/180;

        ctx.strokeStyle = Colors.Green;
        this.context.beginPath();
        ctx.lineWidth = 10;
        this.rotAngle = Math.max(0, this.rotAngle);
        this.rotAngle = Math.min(360, this.rotAngle);
        if (this.rotFlip) {
            ctx.arc(center.x, center.y, 100, angle, 0, false)
        } else {
            ctx.arc(center.x, center.y, 100, 2*Math.PI, angle, false)
        }
        
        ctx.stroke();
    }

    private drawCrosshair() {
        let ctx = this.context;
        let cnv = this.canvas;
        let center = this.canvasCenter();

        ctx.lineWidth = 2;
        ctx.strokeStyle = Colors.Green;
        ctx.beginPath();
        ctx.moveTo(0, center.y);
        ctx.lineTo(cnv.width, center.y)
        ctx.moveTo(center.x, 0);
        ctx.lineTo(center.x, cnv.height);
        ctx.stroke();
    }

    private redraw(): void {
        this.clearCanvas();
        let ctx = this.context;
        let pos = this.mousePos;

        //Draw all components
        this.components.forEach(e => { e.draw(this.context); });

        //this.drawCycle();
        this.drawCrosshair();
        this.drawMouseCircle();

    }

    private objectAt(pos: { x: number, y: number } ): OTSObject {


        return null;
    }

    private moveToFront(obj:Component) {
        if (obj == null) { return null; }

        var idx = this.components.indexOf(obj);
        var rem = this.components.splice(idx, 1);
        this.components.push(rem[0]);
    }

    private drawMouseCircle() {
        if (!this.mouseHold) { return false; }
        let pos = this.mousePos;
        this.context.beginPath();
        this.context.lineWidth = 2;
        this.context.strokeStyle = 'red';
        this.context.arc(pos.x, pos.y, 20, 0, 2*Math.PI);
        this.context.stroke();
    }

    private onMousePress(e: MouseEvent) {

        if (e.button == 0) {//left
            this.mouseHold = true;
            var obj = this.objectAt(this.mousePos);
        } else if (e.button == 1) {//center

        } else if (e.button == 2) {//right

        }

        this.redraw();
    }

    private onMouseRelease(e: MouseEvent) {
        this.mouseHold = false;
        this.redraw();
    }

    private onMouseMove(e: MouseEvent) {
        this.mousePos = this.normalMousePos(e);

        if (this.mouseHold) {

        }
        if (this.selected != null) {

        }

        this.redraw();
    }

    private onMouseLost(e: MouseEvent) {
        this.mousePos = null;
    }
    
    private canvasCenter(): Dim {
        let w = this.canvas.width/2;
        let h = this.canvas.height/2;
        return {x : w, y: h };
    }

    public setOnObjectSelect(callback: (component:Component) => void) 
    { 
        this.onSelectCallback = callback 
    }

    public addComponent(component: Component) {
        var center = this.canvasCenter();
        component.setPos(this.canvasCenter());
        this.components.push(component);
        this.redraw();
    }
}   

export class CanvasController {
    public showAlert() { alert("Test alert") }
        
    canvas: Canvas;
    viewAnchor: [number, number];
    components: Array<Component>;

    constructor() {
        this.viewAnchor = [0, 0];
        this.components = [];
        this.canvas = new Canvas();
    }

    addComponent(component: Component) {
        this.canvas.addComponent(component);
    }
}
