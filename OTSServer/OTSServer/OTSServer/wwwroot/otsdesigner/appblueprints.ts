
export enum Shapes {
    Oval, Rect,
    Cylinder, RoundRect,
    Curve, Line,
}

export enum OTSClass {
    Component = "Component",
    Provider = "Provider",
    Actuator = "Actuator",
    Monitor = "Monitor",
    Single = "Single",
    Nomad = "Nomad",
    Input = "Input", 
    Output = "Output", 
    View = "View",
    Field = "Field"
}

export enum Colors {
    Red = '#c41808', 
    Cyan = '#0bc5d6', 
    Yellow = '#edbb18', 
    Purple = '#890dba', 
    Grey = '#b1adb3', 
    Black = '#000000',
    White = '#FFFFFF',
    Green = '#3ad62f', 
    Orange = '#e66c02', 
    Clear = null
}

export enum Types {
    String, Signed, Unsigned,
    Bool, Decimal, Map, Array
}

export type Dim = { x: number, y:number }
class BoxCoord {
    public top: number;
    public bottom: number;
    public left: number;
    public right: number;

    constructor(pos:Dim, dim:Dim) {
        this.top = pos.y - dim.y/2;
        this.bottom = pos.y + dim.y/2;
        this.left = pos.x - dim.x/2;
        this.right = pos.x + dim.x/2;
    }
}

const DefaultDim: Dim = { x: 50, y:50 }
const ZeroDim: Dim = { x: 0, y: 0 }
const LR_Margin: number = 1.15
const TB_Margin: number = 1.40
const TitleFont: string = '22px serif'
const LabelFont: string = '14px serif'

export class OTSObject {
    pos: Dim;
    dim: Dim;
    id: string;
    otsClass: OTSClass
        
    public inBound(coord: Dim): OTSObject {
        return (
            (coord.x > (this.pos.x-this.dim.x/2)) &&
            (coord.x < (this.pos.x+this.dim.x/2)) &&
            (coord.y > (this.pos.y-this.dim.y/2)) &&
            (coord.y < (this.pos.y+this.dim.y/2))
        ) ? this : null;
    }

    public setPos(pos: Dim, ctx:CanvasRenderingContext2D) {
        this.pos = pos;
        console.log(`POS: ${pos.x}, ${pos.y}`)
    }

    constructor(otsClass:OTSClass, pos:Dim = ZeroDim, dim:Dim = DefaultDim) {
        this.pos = pos;
        this.dim = dim;
        this.id = crypto.randomUUID()
        this.otsClass = otsClass;
    }
}

export class Renderable extends OTSObject {
    shape: Shapes;
    fill: Colors;
    stroke: Colors;
    strokeWidth: number;

    activeStroke: Colors;
    activeFill: Colors;

    constructor(otsClass:OTSClass,
        shape:Shapes, fill:Colors, stroke:Colors,
        strokeWidth: number = 2,
        pos:Dim = ZeroDim, dim:Dim = DefaultDim) {
        super(otsClass, pos, dim);

        this.shape = shape;
        this.fill = fill;
        this.stroke = stroke;
        this.strokeWidth = strokeWidth;
        this.activeFill = fill;
        this.activeStroke = stroke;
    }

    draw(ctx:CanvasRenderingContext2D) {
        
        var finish = function (ctx, stroke, fill) { 
            if (stroke != Colors.Clear) { ctx.stroke(); }
            if (fill != Colors.Clear) { ctx.fill(); } }

        let pos = this.pos;
        let dim = this.dim;
        let lCr:Dim = { x: pos.x-dim.x/2, y: pos.y-dim.y/2 }
        let rCr:Dim = { x: pos.x+dim.x/2, y: pos.y+dim.y/2 }
        ctx.lineWidth = this.strokeWidth;
        ctx.beginPath();
        ctx.strokeStyle = this.activeStroke as string;
        ctx.fillStyle = this.activeFill as string;

        if (this.shape == Shapes.Oval) {
            ctx.arc(pos.x, pos.y, dim.x, 0, 2*Math.PI)

        } else if (this.shape == Shapes.Rect) {
            ctx.rect(lCr.x, lCr.y, dim.x, dim.y);

        } else if (this.shape == Shapes.RoundRect) {
            ctx.roundRect(pos.x, pos.y, dim.x, dim.y, 10)

        } else if (this.shape == Shapes.Cylinder) {
            let ty = pos.y-dim.y/2;
            let by = pos.y+dim.y/2;
            ctx.beginPath()
            ctx.moveTo(lCr.x, ty)
            ctx.bezierCurveTo(lCr.x, ty, pos.x, ty-15, rCr.x, ty)
            ctx.moveTo(lCr.x, ty)
            ctx.bezierCurveTo(lCr.x, ty, pos.x, ty+15, rCr.x, ty)

            ctx.moveTo(lCr.x, by)
            ctx.bezierCurveTo(lCr.x, by, pos.x, by-15, rCr.x, by)
            ctx.moveTo(lCr.x, by)
            ctx.bezierCurveTo(lCr.x, by, pos.x, by+15, rCr.x, by)

            ctx.moveTo(lCr.x, ty)
            ctx.lineTo(lCr.x, by)
            ctx.moveTo(rCr.x, ty)
            ctx.lineTo(rCr.x, by)

        } else if (this.shape == Shapes.Line) {
            ctx.moveTo(pos.x, pos.y)
            ctx.lineTo(dim.x, dim.y)

        } else if (this.shape == Shapes.Curve) {
            let avg = {x: (pos.x+dim.x)/2, y: (pos.y+dim.y)/2 }
            ctx.moveTo(pos.x, pos.y)
            ctx.bezierCurveTo(pos.x, pos.y, avg.x, avg.y, dim.x, dim.y)

        }

        //apply indidually for complex shading options?
        finish(ctx, this.activeStroke, this.activeFill);
    }

    setSelected() {
        console.log('Selected')
    }

    setUnselected() {
        console.log('Unselected')
    }
}

export class Draggable extends Renderable {

    constructor(otsClass:OTSClass,
        shape: Shapes, fill: Colors, stroke: Colors,
        strokeWidth: number = 2,
        pos: Dim = ZeroDim, dim: Dim = DefaultDim) {
            super(otsClass, shape, fill, stroke, strokeWidth, pos, dim);
        }

    drag(delta:Dim, ctx:CanvasRenderingContext2D) {
        let newPos = {
            x: this.pos.x + delta.x,
            y: this.pos.y + delta.y
        }
        this.setPos(newPos, ctx)
    }
}

export class Component extends Draggable{
    
    name: string;
    inputs: Input[];
    outputs: Output[];
    fields: Field[];
    views: View[];
    allowViews: boolean;
    titleDim: Dim;
    boxdim: BoxCoord

    private adjustNodePos(isLeft: boolean, nodes1:Node[], nodes2:Node[], ctx:CanvasRenderingContext2D) {
        let box = this.boxdim;
        let numOuts = this.outputs.length;
        let numIns = this.inputs.length + this.views.length;

        let dy = this.dim.y / (numOuts+1)
        for (let i = 0; i < numOuts; ++i) {
            let output = this.outputs[i]
            let outPos = {x:box.left, y:(i+1)*dy+box.top}
            output.setPos(outPos, ctx)
        }

        dy = this.dim.y / (numIns+1)
        for (let i = 0; i < this.inputs.length; ++i) {
            let input = this.inputs[i]
            let inPos = {x:box.right, y:(i+1)*dy+box.top}
            input.setPos(inPos, ctx)
        }

        for (let i = 0; i < this.views.length; ++i) {
            let view = this.views[i]
            let viewPos = {x:box.right, y:(this.inputs.length+i+1)*dy+box.top}
            view.setPos(viewPos, ctx)
        }

    }

    private adjustFieldPos(ctx:CanvasRenderingContext2D) {
        let dy = (this.dim.y - this.titleDim.y) / (this.fields.length + 1);
        
        for (let i = 0; i < this.fields.length; ++i) {
            let field = this.fields[i];
            let x = this.pos.x
            let y = this.pos.y - this.titleDim.y/2 + field.dim.y * i;
            field.setPos({x:x, y:y}, ctx)
        }
    }

    private setTitle(ctx:CanvasRenderingContext2D) {
        ctx.font = TitleFont;
        let x = this.pos.x - this.titleDim.x/2;
        let y = this.boxdim.top + this.titleDim.y/2;
        ctx.lineWidth = .2;
        ctx.fillStyle = Colors.White;
        ctx.strokeStyle = Colors.Black;
        ctx.fillText(this.name, x, y);
        ctx.strokeText(this.name, x, y);
    }

    private measureText(text:string, font:string, ctx: CanvasRenderingContext2D) {
        ctx.font = font;
        let metrics = ctx.measureText(text);
        return {x: metrics.width, y:metrics.actualBoundingBoxAscent + metrics.actualBoundingBoxDescent}
    }

    private calcReqDim(ctx:CanvasRenderingContext2D) : Dim{

        let numNodes = Math.max(this.outputs.length, this.inputs.length + this.views.length);
        let numViews = this.views.length;
        let textDim = this.measureText(this.name, TitleFont, ctx)

        let nodeHeight = Node.NodeHeight * numNodes;
        let fieldHeight = ((Field.fieldHeight * numViews) + textDim.y);
        let fieldWidth = Field.fieldMinWidth;//this.views.reduce((acc, val) => acc + val.dim.x , 0);

        let height = Math.max(nodeHeight, fieldHeight, DefaultDim.y) * TB_Margin;
        let width = Math.max(fieldWidth, textDim.x, DefaultDim.x) * LR_Margin;

        this.titleDim = textDim;

        return { x: width, y: height }
;
    }

    private adjustParts(ctx:CanvasRenderingContext2D) {
        this.dim = this.calcReqDim(ctx);
        this.boxdim = new BoxCoord(this.pos, this.dim);
        this.adjustNodePos(false, this.inputs, this.views, ctx);
        this.adjustNodePos(true, this.outputs, [], ctx);
        this.adjustFieldPos(ctx);
    }

    public override draw(ctx:CanvasRenderingContext2D) {
        super.draw(ctx);
        this.setTitle(ctx);
        this.inputs.forEach(e => e.draw(ctx))
        this.views.forEach(e => e.draw(ctx))
        this.outputs.forEach(e => e.draw(ctx))
        this.fields.forEach(e => e.draw(ctx))
    }

    private setPartHosts() {
        for (let i = 0; i < this.inputs.length; ++i) { this.inputs[i].host = this; }
        for (let i = 0; i < this.outputs.length; ++i) { this.outputs[i].host = this; }
        for (let i = 0; i < this.views.length; ++i) { this.views[i].host = this; }
        for (let i = 0; i < this.fields.length; ++i) { this.fields[i].host = this; }
    }
    
    constructor(
        otsClass:OTSClass,
        name: string,
        inputs: Input[],
        outputs: Output[],
        fields: Field[],
        expandable: boolean,
        shape: Shapes, fill: Colors, stroke: Colors,
        strokeWidth: number = 2,
        pos: Dim = ZeroDim, dim: Dim = DefaultDim) {
        super(otsClass, shape, fill, stroke, strokeWidth, pos, dim);
        
        this.name = name;
        this.inputs = inputs;
        this.outputs = outputs;
        this.fields = fields;
        this.views = [];
        this.allowViews = expandable;
        this.titleDim = { x:100, y:30 }
        this.setPartHosts();
    }

    public drag(deltaPos: Dim, ctx:CanvasRenderingContext2D) {
        super.drag(deltaPos, ctx);
        this.adjustParts(ctx);
    }

    public setPos(pos: Dim, ctx:CanvasRenderingContext2D) {
        super.setPos(pos, ctx)
        this.adjustParts(ctx)
    }
    
    public override inBound(coord: Dim): OTSObject {
        if (super.inBound(coord) === null) { return null; }
        for (let i = 0; i < this.inputs.length; ++i) { if(this.inputs[i].inBound(coord) !== null) return this.inputs[i]; }
        for (let i = 0; i < this.outputs.length; ++i) { if(this.outputs[i].inBound(coord) !== null) return this.outputs[i]; }
        for (let i = 0; i < this.views.length; ++i) { if(this.views[i].inBound(coord) !== null) return this.views[i]; }
        for (let i = 0; i < this.fields.length; ++i) { if(this.fields[i].inBound(coord) !== null) return this.fields[i]; }
        console.log('Return self')
        return this;
    }
}

export class Provider extends Component{
   constructor(
        name: string,
        outputs: Output[],
        fields: Field[],
        ) {

        super(OTSClass.Provider, name, [], outputs, fields, false,
            Shapes.Cylinder, Colors.Clear, Colors.White);
    }
}

export class ComponentPart extends Renderable {
    host: Component;

    constructor(otsClass: OTSClass,
        shape:Shapes, fill: Colors, stroke: Colors, 
        strokeWidth:number, pos:Dim, dim:Dim) {
        super(otsClass, shape, fill, stroke, strokeWidth, pos, dim)
        this.host = null;
    }
}

export class Node extends ComponentPart {

    static readonly NodeHeight = 4;
    static readonly NodeWidth = 4;
    constructor(otsClass: OTSClass, fill:Colors, stroke:Colors) {
        super(otsClass, Shapes.Oval, fill, stroke, 3, undefined, {x:Node.NodeWidth, y:Node.NodeHeight})
    }

    public override draw(ctx: CanvasRenderingContext2D) {
        super.draw(ctx)
    }
}

export class Output extends Node {
    constructor() {
        super(OTSClass.Output, Colors.Clear, Colors.Yellow);
    }
}

export class Input extends Node {
    constructor(fill:Colors=Colors.Clear, stroke:Colors=Colors.Green) {
        super(OTSClass.Input, fill, stroke);
    }
}

export class View extends Input {
    constructor() {
        super(undefined, Colors.Yellow);
    }
}

export class Field extends ComponentPart {

    public static readonly fieldHeight: number = 18;
    public static readonly fieldMinWidth: number = 120;
    public valPair: ValPair;
    public name: string;
    constructor(type:Types) {
        super(OTSClass.Field, Shapes.Rect, Colors.White, Colors.Black, 2, undefined, {x:Field.fieldMinWidth, y:Field.fieldHeight});
        this.valPair = defaultValue(type);
    }

    public override draw(ctx: CanvasRenderingContext2D) {
        
        super.draw(ctx);
    }
}

function defaultValue(type: Types) : ValPair{
    let val:any = null;
    switch (type) {
        case Types.Signed:
        case Types.Unsigned:
            val = 0; break;
        case Types.Decimal:
            val = 0.0; break;
        case Types.Bool:
            val = false; break;
        case Types.String:
            val = ''; break;
        case Types.Array:
            val = []; break;
        case Types.Map:
            val = { }; break;
    }

    return { value: val, type: type };
}

function getType(value): Types {
    if (typeof value === 'number') {
        if (value % Math.trunc(value) != 0) { return Types.Decimal; }
        return value < 0 ? Types.Signed : Types.Unsigned;
    }
    if (typeof value === 'string') {
        return Types.String;
    }
    if (typeof value === 'boolean') { return Types.Bool; }
    return value.constructor == Object? Types.Map : Types.Array;
}

export type ValPair = { value: any, type: Types }
function getValuePairFrom(type: Types, value: any) : ValPair {
        
    let ret:ValPair = { value: 0, type:Types.Signed };

    switch (type) {
        case Types.Signed: {

            break;
        }
        case Types.Unsigned: {
            break;
        }
        case Types.Bool: {
            break;
        }
    }

    return ret;
}

export function deltaPos(pos1: Dim, pos2: Dim): Dim {
    return { x: pos2.x-pos1.x, y: pos2.y-pos1.y }
}