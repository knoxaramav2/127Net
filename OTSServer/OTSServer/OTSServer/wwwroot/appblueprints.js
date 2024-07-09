export var otsdesigner;
(function (otsdesigner) {
    let OTSClass;
    (function (OTSClass) {
        OTSClass[OTSClass["Actuator"] = 0] = "Actuator";
        OTSClass[OTSClass["Single"] = 1] = "Single";
        OTSClass[OTSClass["Nomad"] = 2] = "Nomad";
        OTSClass[OTSClass["Provider"] = 3] = "Provider";
        OTSClass[OTSClass["Monitor"] = 4] = "Monitor";
    })(OTSClass = otsdesigner.OTSClass || (otsdesigner.OTSClass = {}));
    let Shapes;
    (function (Shapes) {
        Shapes[Shapes["Circle"] = 0] = "Circle";
        Shapes[Shapes["Square"] = 1] = "Square";
        Shapes[Shapes["Oval"] = 2] = "Oval";
        Shapes[Shapes["Rect"] = 3] = "Rect";
    })(Shapes || (Shapes = {}));
    let Colors;
    (function (Colors) {
    })(Colors || (Colors = {}));
    class renderable {
        draw(ctx, shape, stroke, strokeWidth, fill, offset, scale) {
        }
    }
    class draggable {
        moveTo(pos) {
            this.pos = pos;
        }
    }
    class ConfigField {
        draw(ctx) {
        }
    }
    otsdesigner.ConfigField = ConfigField;
    class IONode {
        draw(ctx) {
        }
    }
    class InputNode extends IONode {
    }
    class ViewNode extends InputNode {
    }
    class OutputNode extends IONode {
    }
    class Link extends draggable {
        constructor(input, output) {
            super();
            this.input = input;
            this.output = output;
            this.pos = { x: 0, y: 0 };
        }
    }
    class OTSObj extends draggable {
        draw(ctx) {
        }
    }
    otsdesigner.OTSObj = OTSObj;
    class IOSiding {
        constructor() {
            this.nodes = new Array();
        }
        draw(ctx) {
        }
        addNode(node) {
            this.nodes.push(node);
        }
        popNode() {
            this.nodes.pop();
        }
        removeNode(node) {
            var idx = this.nodes.indexOf(node);
            if (idx != -1) {
                this.nodes.splice(idx, 1);
            }
        }
    }
    class InputSiding extends IOSiding {
    }
    class OutputSiding extends IOSiding {
    }
    class ViewSiding extends IOSiding {
    }
    class OTSComponent extends OTSObj {
        constructor() {
            super(...arguments);
            this.fields = [];
        }
    }
    otsdesigner.OTSComponent = OTSComponent;
    //Has inputs and outputs
    //May or may not have expandable inputs
    //Primary middle layer component
    class OTSActuator extends OTSComponent {
        constructor() {
            super(...arguments);
            this.inputs = new InputSiding();
            this.outputs = new OutputSiding();
            this.views = new ViewSiding();
        }
    }
    //Only outputs
    class OTSProvider extends OTSComponent {
        constructor() {
            super(...arguments);
            this.outputs = new OutputSiding();
        }
    }
    //Only inputs
    //Inputs may be expandable
    class OTSMonitor extends OTSComponent {
        constructor() {
            super(...arguments);
            this.inputs = new InputSiding();
            this.outputs = new OutputSiding();
            this.views = new ViewSiding();
        }
    }
    //Has ouputs
    //Only expandable inputs
    class OTSNomad extends OTSComponent {
    }
    //No inputs or outputs
    class OTSSingle extends OTSComponent {
    }
})(otsdesigner || (otsdesigner = {}));
//# sourceMappingURL=appblueprints.js.map