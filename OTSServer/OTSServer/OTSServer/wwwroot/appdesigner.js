var otsdesigner;
(function (otsdesigner) {
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
            this.components = [];
        }
        constructor() {
            console.log('Readying canvas....');
            this.initCanvas();
            this.initEventListeners();
            console.log(this.canvas == null ? 'Failed' : 'Success');
            this.initMisc();
            this.initCallbacks();
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
        redraw() {
            this.clearCanvas();
            let context = this.context;
            let pos = this.mousePos;
            context.beginPath();
            context.strokeStyle = 'red';
            context.arc(pos.x, pos.y, 20, 0, 2 * Math.PI);
            context.stroke();
            console.log('draw');
        }
        objectAt(pos) {
        }
        moveToFront() {
        }
        onMousePress(e) {
            if (e.button == 0) { //left
            }
            else if (e.button == 1) { //center
            }
            else if (e.button == 2) { //right
            }
        }
        onMouseRelease(e) {
        }
        onMouseMove(e) {
            this.mousePos = this.normalMousePos(e);
            if (this.selected != null) {
            }
            this.redraw();
        }
        onMouseLost(e) {
            this.mousePos = null;
        }
        drawComponent(name, inputs, outputs) {
        }
        setOnObjectSelect(callback) { this.onSelectCallback = callback; }
    }
    class AppDesignController {
        showAlert() { alert("Test alert"); }
        constructor() {
            this.viewAnchor = [0, 0];
            this.components = [];
            this.canvas = new Canvas();
        }
        addComponent(name) {
            console.log(`Create ${name}`);
        }
    }
    function initAppController() {
        return new AppDesignController();
    }
    otsdesigner.initAppController = initAppController;
})(otsdesigner || (otsdesigner = {}));
export {};
//# sourceMappingURL=appdesigner.js.map