import { otsdesigner as bp } from "./appblueprints";
import OTSComponent = bp.OTSComponent;
import OTSObj = bp.OTSObj;
import OTSField = bp.ConfigField;

namespace otsdesigner{
    class Canvas {
        canvas: HTMLCanvasElement;
        mousePos: {x:number, y:number};
        context: CanvasRenderingContext2D;

        selected: OTSComponent;
        components: Array<OTSComponent>;

        //Callbacks
        onSelectCallback: (component:OTSComponent) => void;
        onEditFieldSelectCallback: (field:OTSField) => void;

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
            this.components = [];
        }

        constructor() {
            
            console.log('Readying canvas....')
            this.initCanvas();
            this.initEventListeners();
            console.log(this.canvas == null ? 'Failed' : 'Success');
            this.initMisc();
            this.initCallbacks();
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

        private redraw(): void {
            this.clearCanvas();
            let context = this.context;
            let pos = this.mousePos;

            context.beginPath();
            context.strokeStyle = 'red';
            context.arc(pos.x, pos.y, 20, 0, 2*Math.PI);
            context.stroke();

            console.log('draw')
        }

        private objectAt(pos: { x: number, y: number } ) {

        }

        private moveToFront() {

        }

        private onMousePress(e: MouseEvent) {

            if (e.button == 0) {//left

            } else if (e.button == 1) {//center

            } else if (e.button == 2) {//right

            }

        }

        private onMouseRelease(e: MouseEvent) {

        }

        private onMouseMove(e: MouseEvent) {
            this.mousePos = this.normalMousePos(e);

            if (this.selected != null) {

            }

            this.redraw();
        }

        private onMouseLost(e: MouseEvent) {
            this.mousePos = null;
        }
    
        public drawComponent(
            name:string,
            inputs: number,
            outputs: number,
            ) {

        }

        public setOnObjectSelect(callback: (component:OTSComponent) => void) { this.onSelectCallback = callback }
    }   

    class AppDesignController {
        public showAlert() { alert("Test alert") }
        
        canvas: Canvas;
        viewAnchor: [number, number];
        components: Array<OTSComponent>;

        constructor() {
            this.viewAnchor = [0, 0];
            this.components = [];
            this.canvas = new Canvas();
        }

        addComponent(name:string) {
            console.log(`Create ${name}`)
        }
    }

    export function initAppController(): AppDesignController{
        return new AppDesignController();
    }
}
