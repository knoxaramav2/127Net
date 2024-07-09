 export namespace otsdesigner{

     export enum OTSClass {
         Actuator, Single,
         Nomad, Provider,
         Monitor
     }

    enum Shapes {
        Circle, Square, 
        Oval, Rect,
    }

     enum Colors {

     }

    class renderable {

        draw(
            ctx: CanvasRenderingContext2D, 
            shape:Shapes, 
            stroke:string|Colors, strokeWidth:number,
            fill:string|Colors,
            offset: { x: number, y: number }, 
            scale: number) {

        }
    }

    class draggable {
        pos: {x:number, y:number}

        moveTo(pos:{x:number, y:number}) {
            this.pos = pos;
        }
    }
    export class ConfigField implements renderable {
        value: string;

        draw(ctx:CanvasRenderingContext2D) {

        }
    }

    class IONode implements renderable {
        draw(ctx:CanvasRenderingContext2D) {
            
        }
    }

    class InputNode extends IONode {

    }

    class ViewNode extends InputNode {

    }

    class OutputNode extends IONode  {

    }

    class Link extends draggable {
        input: InputNode;
        output: OutputNode;

        constructor(input: InputNode, output: OutputNode) {
            super();
            this.input = input;
            this.output = output;
            this.pos = { x:0, y:0 };
        }
    }

    export class OTSObj extends draggable implements renderable {
        name: string;
        position: [number, number];
        configFields: ConfigField[];
        class: OTSClass;

        draw(ctx:CanvasRenderingContext2D) {

        }

        
    }

    class IOSiding implements renderable {
        nodes: Array<IONode> = new Array();

        draw(ctx:CanvasRenderingContext2D) {

        }

        protected addNode(node: IONode) {
            this.nodes.push(node);
        }

        protected popNode() {
            this.nodes.pop();
        }

        protected removeNode(node:IONode) {
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

    export class OTSComponent extends OTSObj {
        fields: Array<ConfigField> = [];
    }

    //Has inputs and outputs
    //May or may not have expandable inputs
    //Primary middle layer component
    class OTSActuator extends OTSComponent {
        inputs: InputSiding = new InputSiding();
        outputs: OutputSiding = new OutputSiding();
        views: ViewSiding = new ViewSiding();
    }

    //Only outputs
    class OTSProvider extends OTSComponent {
        outputs: OutputSiding = new OutputSiding();
    }

    //Only inputs
    //Inputs may be expandable
    class OTSMonitor extends OTSComponent {
        inputs: InputSiding = new InputSiding();
        outputs: OutputSiding = new OutputSiding();
        views: ViewSiding = new ViewSiding();
    }

    //Has ouputs
    //Only expandable inputs
    class OTSNomad extends OTSComponent {

    }

    //No inputs or outputs
    class OTSSingle extends OTSComponent {

    }
    
}