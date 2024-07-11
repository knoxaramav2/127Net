
export enum Shapes {
    Oval, Rect,
    Cylinder, RoundRect,
    Curve, Line,
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

enum Types {
    String, Signed, Unsigned,
    Bool, Decimal, Map, Array
}

export type Dim = { x: number, y:number }
const defaultDim: Dim = { x: 50, y:50 }
const zeroDim: Dim = { x: 0, y: 0 }

export class OTSObject {
    pos: Dim;
    dim: Dim;
        
    public inBound(coord: Dim): boolean {

        return (
            (coord.x > (this.pos.x-this.dim.x/2)) &&
            (coord.x < (this.pos.x+this.dim.x/2)) &&
            (coord.y > (this.pos.y-this.dim.y/2)) &&
            (coord.y < (this.pos.y+this.dim.y/2))
        );
    }

    public setPos(pos: Dim) {
        this.pos = pos;
    }

    constructor(pos:Dim = zeroDim, dim:Dim = defaultDim) {
        this.pos = pos;
        this.dim = dim;
    }
}

export class Renderable extends OTSObject {
    shape: Shapes;
    fill: Colors;
    stroke: Colors;
    strokeWidth: number;

    activeStroke: Colors;
    activeFill: Colors;

    constructor(
        shape:Shapes, fill:Colors, stroke:Colors,
        strokeWidth: number = 2,
        pos:Dim = zeroDim, dim:Dim = defaultDim) {
        super(pos, dim);

        this.shape = shape;
        this.fill = fill;
        this.stroke = stroke;
        this.strokeWidth = strokeWidth;
        this.activeFill = fill;
        this.activeStroke = stroke;
    }

    draw(ctx:CanvasRenderingContext2D) {
        
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

            ctx.stroke();
        } else if (this.shape == Shapes.Line) {

        } else if (this.shape == Shapes.Curve) {

        }

        if (this.activeStroke != Colors.Clear) { ctx.stroke(); }
        if (this.activeFill != Colors.Clear) { ctx.fill(); }
    }
}

export class Draggable extends Renderable {

    constructor(
        shape: Shapes, fill: Colors, stroke: Colors,
        strokeWidth: number = 2,
        pos: Dim = zeroDim, dim: Dim = defaultDim) {
            super(shape, fill, stroke, strokeWidth, pos, dim);
        }

    dragTo(delta:Dim) {
        this.pos.x += delta.x;
        this.pos.y += delta.y;
    }
}

export class Component extends Renderable{
    
    name: string;
    inputs: Input[];
    outputs: Output[];
    fields: Field[];
    views: View[];
    allowViews: boolean;

    private adjustNodePos(isLeft: boolean, nodes1:Node[], nodes2:Node[]) {
        let x = isLeft? this.pos.x-this.dim.x/2 : this.pos.x+this.dim.x/2;
        let numNodes = nodes1.length + nodes2.length;
        let dy = this.dim.y / (numNodes+1)
        let ty = this.pos.y-this.dim.y/2

        for (let i = 0; i < nodes1.length; ++i) {
            nodes1[i].setPos({ x:x, y: ty+dy*(i+1)})
        }

        for (let i = 0; i < nodes2.length; ++i) {
            nodes1[i].setPos({ x:x, y: ty+dy*(i+nodes1.length+1)})
        }
    }

    private adjustFieldPos() {
        
    }

    private adjustParts() {
        this.adjustNodePos(false, this.inputs, this.views);
        this.adjustNodePos(true, this.outputs, []);
        this.adjustFieldPos()
    }

    public override draw(ctx:CanvasRenderingContext2D) {
        super.draw(ctx);
        this.inputs.forEach(e => e.draw(ctx))
        this.views.forEach(e => e.draw(ctx))
        this.outputs.forEach(e => e.draw(ctx))
        this.fields.forEach(e => e.draw(ctx))
    }
    
    constructor(
        name: string,
        inputs: Input[],
        outputs: Output[],
        fields: Field[],
        expandable: boolean,
        shape: Shapes, fill: Colors, stroke: Colors,
        strokeWidth: number = 2,
        pos: Dim = zeroDim, dim: Dim = defaultDim) {
        super(shape, fill, stroke, strokeWidth, pos, dim);

        this.name = name;
        this.inputs = inputs;
        this.outputs = outputs;
        this.fields = fields = [];
        this.views = [];
        this.allowViews = expandable;

        this.adjustParts();

    }

    public override setPos(pos: Dim) {
        super.setPos(pos)
        this.adjustParts()
    }
    
}

export class Provider extends Component{
   constructor(
        name: string,
        outputs: Output[],
        fields: Field[],
        ) {

        super(name, [], outputs, fields, false,
            Shapes.Cylinder, Colors.Clear, Colors.White);
    }
}

export class Node extends Renderable {
    constructor(fill:Colors, stroke:Colors) {
        super(Shapes.Oval, fill, stroke, 3, undefined, {x:4, y:4})
    }

    public override draw(ctx: CanvasRenderingContext2D) {
        super.draw(ctx)
    }
}

export class Output extends Node {
    constructor() {
        super(Colors.Clear, Colors.Yellow);
    }
}

export class Input extends Node {
    constructor(fill:Colors=Colors.Clear, stroke:Colors=Colors.Green) {
        super(fill, stroke);
    }
}

export class View extends Input {
    constructor() {
        super(undefined, Colors.Yellow);
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

export class Field extends Renderable {
    public valPair: ValPair;
    public name: string;
    constructor(type:Types) {
        super(Shapes.Rect, Colors.White, Colors.Black, 2);
        this.valPair = defaultValue(type);
    }
}
