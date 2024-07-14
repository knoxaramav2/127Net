﻿import { 
    Component, Renderable,
    Field, OTSObject,
    Dim, Colors,
    deltaPos, scaleLine
} from "./appblueprints.js";

enum DragMode {
    None,
    Left,
    Middle
} 

class Canvas {
    canvas: HTMLCanvasElement;
    context: CanvasRenderingContext2D;

    selected: Renderable
    components: Array<Component>;

    //Callbacks
    onEditFieldSelectCallback: (field:Field) => void;

    //state
    mouseHold: DragMode;
    mousePos: Dim;
    updateSpeed: number;
    //cycleCenter: Dim;
    rotAngle:number;
    rotSpeed:number;
    rotFlip: boolean;
    canvasCenter: Dim;
    canvasScale: number;

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
        this.onMouseWheel = this.onMouseWheel.bind(this);
        this.onScaleChange = this.onScaleChange.bind(this);

        this.canvas.addEventListener("mousedown", this.onMousePress);
        this.canvas.addEventListener("mouseup", this.onMouseRelease);
        this.canvas.addEventListener("mousemove", this.onMouseMove);
        this.canvas.addEventListener("mouseout", this.onMouseLost);
        this.canvas.addEventListener("wheel", this.onMouseWheel);
        window.addEventListener('resize', this.onScaleChange);
    }

    private initMisc() {
        this.mousePos = null;
        this.selected = null;
        this.mouseHold = DragMode.None;
        this.components = [];

        this.updateSpeed = 0.01;

        //this.cycleCenter = this.getCanvasCenter();
        this.rotAngle = 0.0;
        this.rotSpeed = 2.5;
        this.rotFlip = false;
        this.canvasCenter = this.getCanvasCenter();
        this.canvasScale = 1.0;
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

    private drawCrosshair() {
        let ctx = this.context;
        let cnv = this.canvas;
        let center = this.canvasCenter

        ctx.lineWidth = 2;
        ctx.strokeStyle = Colors.Green;
        ctx.globalAlpha = .3
        ctx.beginPath();
        ctx.moveTo(0, center.y);
        ctx.lineTo(cnv.width, center.y)
        ctx.moveTo(center.x, 0);
        ctx.lineTo(center.x, cnv.height);
        ctx.stroke();

        let offs = this.canvasScale
        let offDim = {x: 100, y: 100}
        let ncenter = scaleLine(center, offDim, this.canvasScale)

        ctx.beginPath();
        ctx.strokeStyle = Colors.Yellow;
        ctx.moveTo(ncenter.x, ncenter.y);
        ctx.lineTo(ncenter.x+offs, ncenter.y+offs);
        ctx.stroke();

        ncenter = center;//this.getCanvasCenter();
        ctx.beginPath();
        ctx.strokeStyle = Colors.Purple;
        ctx.moveTo(ncenter.x, ncenter.y);
        ctx.lineTo(ncenter.x+offs, ncenter.y+offs);
        ctx.stroke();

        ctx.globalAlpha = 1
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

    private objectAt(pos: { x: number, y: number } ): Renderable {

        for (let i = this.components.length - 1; i >= 0; --i) {
            let cmp = this.components[i];
            if (cmp.inBound(pos)) {
                return cmp;
            }
        }

        return null;
    }

    private moveToFront(obj:Renderable) {
        if (obj == null) { return null; }
        let cmp = obj as Component;
        var idx = this.components.indexOf(cmp);
        var rem = this.components.splice(idx, 1);
        this.components.push(rem[0]);
    }

    private drawMouseCircle() {
        if (!this.mouseHold) { return false; }
        let pos = this.mousePos;
        if (pos == null) { return; }
        this.context.beginPath();
        this.context.lineWidth = 2;
        this.context.strokeStyle = 'red';
        this.context.arc(pos.x, pos.y, 20, 0, 2*Math.PI);
        this.context.stroke();
    }

    private translateCanvas(delta:Dim) {
        this.canvasCenter = {
            x:this.canvasCenter.x+delta.x , 
            y:this.canvasCenter.y+delta.y
        }

        for (let i = 0; i < this.components.length; ++i) { 
            this.components[i].drag(delta, this.context); 
        }
    }

    private onMousePress(e: MouseEvent) {

        if (e.button == 0) {//left
            this.mouseHold = DragMode.Left;
            var obj = this.objectAt(this.mousePos);
            if (obj !== null) {
                if (this.selected !== null && this.selected !== obj) {
                    this.selected.setUnselected();
                    console.log(`UNSELECT: ${this.selected.otsClass}`)
                }

                this.selected = obj;
                this.selected.setSelected();
                this.moveToFront(this.selected);

                console.log(`SELECT: ${obj.otsClass}`)
                
            } else if (this.selected !== null) {
                this.selected.setUnselected();
                this.selected = null;
            }
        } else if (e.button == 1) {//center
            this.mouseHold = DragMode.Middle;
        } else if (e.button == 2) {//right
            
        }

        this.redraw();
    }

    private onMouseRelease(e: MouseEvent) {
        this.mouseHold = DragMode.None;
        this.redraw();
    }

    private onMouseMove(e: MouseEvent) {
        let oldMouse = this.mousePos ?? this.normalMousePos(e);
        this.mousePos = this.normalMousePos(e);
        let selected = this.selected;
        let delta = deltaPos(oldMouse, this.mousePos);
        if (this.mouseHold === DragMode.Left) {
            if (selected !== null) {
                //drag
                if (selected instanceof Component) {
                    (selected as Component).drag(delta, this.context)
                }
                //DrawLink
                else if (selected instanceof Node) {
                    console.log('Selected node')
                    //TODO
                }
            }
        } else if (this.mouseHold === DragMode.Middle) {
            this.translateCanvas(delta);
        }

        if (this.selected != null) {

        }

        this.redraw();
    }

    private onMouseLost(e: MouseEvent) {
        this.mousePos = null;
    }

    
    static readonly scrollScalar = 0.01;
    private onMouseWheel(e: WheelEvent) {
        let ds = e.deltaY > 0 ? -Canvas.scrollScalar : Canvas.scrollScalar;
        this.canvasScale += ds;

        let mpos: Dim = { x:e.x, y:e.y }
        let screenCenter = this.getCanvasCenter();
        let canvasCenter = this.canvasCenter;

        for (let i = 0; i < this.components.length; ++i) { 
            let cmp = this.components[i];
            let npos = scaleLine(canvasCenter, cmp.pos, this.canvasScale);

            cmp.setScale(this.canvasScale, this.context);
            cmp.setPos(npos, this.context);
        }
        this.redraw();
    }

    private getCanvasCenter(): Dim {
        let w = this.canvas.width/2;
        let h = this.canvas.height/2;
        return {x : w, y: h };
    }

    public addComponent(component: Component) {
        var center = this.getCanvasCenter();
        component.setPos(this.getCanvasCenter(), this.context);
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

        component.setScale(this.canvas.canvasScale, this.canvas.context)
        this.canvas.addComponent(component);
    }
}