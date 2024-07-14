export var Shapes;
(function (Shapes) {
    Shapes[Shapes["Oval"] = 0] = "Oval";
    Shapes[Shapes["Rect"] = 1] = "Rect";
    Shapes[Shapes["Cylinder"] = 2] = "Cylinder";
    Shapes[Shapes["RoundRect"] = 3] = "RoundRect";
    Shapes[Shapes["Curve"] = 4] = "Curve";
    Shapes[Shapes["Line"] = 5] = "Line";
})(Shapes || (Shapes = {}));
export var OTSClass;
(function (OTSClass) {
    OTSClass["Component"] = "Component";
    OTSClass["Provider"] = "Provider";
    OTSClass["Actuator"] = "Actuator";
    OTSClass["Monitor"] = "Monitor";
    OTSClass["Single"] = "Single";
    OTSClass["Nomad"] = "Nomad";
    OTSClass["Input"] = "Input";
    OTSClass["Output"] = "Output";
    OTSClass["View"] = "View";
    OTSClass["Field"] = "Field";
})(OTSClass || (OTSClass = {}));
export var Colors;
(function (Colors) {
    Colors["Red"] = "#c41808";
    Colors["Cyan"] = "#0bc5d6";
    Colors["Yellow"] = "#edbb18";
    Colors["Purple"] = "#890dba";
    Colors["Grey"] = "#b1adb3";
    Colors["Black"] = "#000000";
    Colors["White"] = "#FFFFFF";
    Colors["Green"] = "#3ad62f";
    Colors["Orange"] = "#e66c02";
    Colors[Colors["Clear"] = null] = "Clear";
})(Colors || (Colors = {}));
export var Types;
(function (Types) {
    Types[Types["String"] = 0] = "String";
    Types[Types["Signed"] = 1] = "Signed";
    Types[Types["Unsigned"] = 2] = "Unsigned";
    Types[Types["Bool"] = 3] = "Bool";
    Types[Types["Decimal"] = 4] = "Decimal";
    Types[Types["Map"] = 5] = "Map";
    Types[Types["Array"] = 6] = "Array";
})(Types || (Types = {}));
class BoxCoord {
    constructor(pos, dim) {
        this.top = pos.y - dim.y / 2;
        this.bottom = pos.y + dim.y / 2;
        this.left = pos.x - dim.x / 2;
        this.right = pos.x + dim.x / 2;
    }
}
const DefaultDim = { x: 50, y: 50 };
const ZeroDim = { x: 0, y: 0 };
const LR_Margin = 1.15;
const TB_Margin = 1.40;
const DefaultTitleFont = '22px serif';
const DefaultLabelFont = '14px serif';
export class OTSObject {
    inBound(coord) {
        return ((coord.x > (this.pos.x - this.dim.x / 2)) &&
            (coord.x < (this.pos.x + this.dim.x / 2)) &&
            (coord.y > (this.pos.y - this.dim.y / 2)) &&
            (coord.y < (this.pos.y + this.dim.y / 2))) ? this : null;
    }
    setPos(pos, ctx) {
        this.pos = pos;
    }
    setScale(scale, ctx) {
        this.scale = scale;
        this.dim = {
            x: this.origDim.x * this.scale,
            y: this.origDim.y * this.scale
        };
    }
    constructor(otsClass, pos = ZeroDim, dim = DefaultDim) {
        this.pos = pos;
        this.dim = dim;
        this.origDim = dim;
        this.id = crypto.randomUUID();
        this.otsClass = otsClass;
        this.scale = 1.0;
    }
}
export class Renderable extends OTSObject {
    titleFont() {
        let fontSz = 22;
        return `${fontSz * this.scale}px Serif`;
    }
    labelFont() {
        let fontSz = 12;
        return `${fontSz * this.scale}px Serif`;
    }
    constructor(otsClass, shape, fill, stroke, strokeWidth = 2, pos = ZeroDim, dim = DefaultDim) {
        super(otsClass, pos, dim);
        this.shape = shape;
        this.fill = fill;
        this.stroke = stroke;
        this.strokeWidth = strokeWidth;
        this.activeFill = fill;
        this.activeStroke = stroke;
    }
    draw(ctx) {
        var finish = function (ctx, stroke, fill) {
            if (stroke != Colors.Clear) {
                ctx.stroke();
            }
            if (fill != Colors.Clear) {
                ctx.fill();
            }
        };
        let pos = this.pos;
        let dim = this.dim;
        let lCr = { x: pos.x - dim.x / 2, y: pos.y - dim.y / 2 };
        let rCr = { x: pos.x + dim.x / 2, y: pos.y + dim.y / 2 };
        ctx.lineWidth = this.strokeWidth;
        ctx.beginPath();
        ctx.strokeStyle = this.activeStroke;
        ctx.fillStyle = this.activeFill;
        if (this.shape == Shapes.Oval) {
            ctx.arc(pos.x, pos.y, dim.x, 0, 2 * Math.PI);
        }
        else if (this.shape == Shapes.Rect) {
            ctx.rect(lCr.x, lCr.y, dim.x, dim.y);
        }
        else if (this.shape == Shapes.RoundRect) {
            ctx.roundRect(pos.x, pos.y, dim.x, dim.y, 10);
        }
        else if (this.shape == Shapes.Cylinder) {
            let ty = pos.y - dim.y / 2;
            let by = pos.y + dim.y / 2;
            ctx.beginPath();
            ctx.moveTo(lCr.x, ty);
            ctx.bezierCurveTo(lCr.x, ty, pos.x, ty - 15, rCr.x, ty);
            ctx.moveTo(lCr.x, ty);
            ctx.bezierCurveTo(lCr.x, ty, pos.x, ty + 15, rCr.x, ty);
            ctx.moveTo(lCr.x, by);
            ctx.bezierCurveTo(lCr.x, by, pos.x, by - 15, rCr.x, by);
            ctx.moveTo(lCr.x, by);
            ctx.bezierCurveTo(lCr.x, by, pos.x, by + 15, rCr.x, by);
            ctx.moveTo(lCr.x, ty);
            ctx.lineTo(lCr.x, by);
            ctx.moveTo(rCr.x, ty);
            ctx.lineTo(rCr.x, by);
        }
        else if (this.shape == Shapes.Line) {
            ctx.moveTo(pos.x, pos.y);
            ctx.lineTo(dim.x, dim.y);
        }
        else if (this.shape == Shapes.Curve) {
            let avg = { x: (pos.x + dim.x) / 2, y: (pos.y + dim.y) / 2 };
            ctx.moveTo(pos.x, pos.y);
            ctx.bezierCurveTo(pos.x, pos.y, avg.x, avg.y, dim.x, dim.y);
        }
        //apply indidually for complex shading options?
        finish(ctx, this.activeStroke, this.activeFill);
    }
    setSelected() {
        console.log('Selected');
    }
    setUnselected() {
        console.log('Unselected');
    }
}
export class Draggable extends Renderable {
    constructor(otsClass, shape, fill, stroke, strokeWidth = 2, pos = ZeroDim, dim = DefaultDim) {
        super(otsClass, shape, fill, stroke, strokeWidth, pos, dim);
    }
    drag(delta, ctx) {
        let newPos = {
            x: this.pos.x + delta.x,
            y: this.pos.y + delta.y
        };
        this.setPos(newPos, ctx);
    }
}
export class Component extends Draggable {
    adjustNodePos(isLeft, nodes1, nodes2, ctx) {
        let box = this.boxdim;
        let numOuts = this.outputs.length;
        let numIns = this.inputs.length + this.views.length;
        let dy = this.dim.y / (numOuts + 1);
        for (let i = 0; i < numOuts; ++i) {
            let output = this.outputs[i];
            let outPos = { x: box.left, y: (i + 1) * dy + box.top };
            output.setPos(outPos, ctx);
        }
        dy = this.dim.y / (numIns + 1);
        for (let i = 0; i < this.inputs.length; ++i) {
            let input = this.inputs[i];
            let inPos = { x: box.right, y: (i + 1) * dy + box.top };
            input.setPos(inPos, ctx);
        }
        for (let i = 0; i < this.views.length; ++i) {
            let view = this.views[i];
            let viewPos = { x: box.right, y: (this.inputs.length + i + 1) * dy + box.top };
            view.setPos(viewPos, ctx);
        }
    }
    adjustFieldPos(ctx) {
        let dy = (this.dim.y - this.titleDim.y) / (this.fields.length + 1);
        for (let i = 0; i < this.fields.length; ++i) {
            let field = this.fields[i];
            let x = this.pos.x;
            let y = this.pos.y - this.titleDim.y / 2 + field.dim.y * i;
            field.setPos({ x: x, y: y }, ctx);
        }
    }
    setTitle(ctx) {
        let font = this.titleFont();
        ctx.font = font;
        let x = this.pos.x - this.titleDim.x / 2;
        let y = this.boxdim.top + this.titleDim.y / 2;
        let dim = this.measureText(this.name, font, ctx);
        ctx.beginPath();
        ctx.rect(x, y - dim.y, dim.x, dim.y);
        ctx.fillStyle = Colors.Black;
        ctx.fill();
        //ctx.stroke();
        ctx.lineWidth = .2;
        ctx.fillStyle = Colors.White;
        ctx.strokeStyle = Colors.Black;
        ctx.fillText(this.name, x, y);
        ctx.strokeText(this.name, x, y);
    }
    measureText(text, font, ctx) {
        ctx.font = font;
        let metrics = ctx.measureText(text);
        return { x: metrics.width, y: metrics.actualBoundingBoxAscent + metrics.actualBoundingBoxDescent };
    }
    calcReqDim(ctx) {
        let numNodes = Math.max(this.outputs.length, this.inputs.length + this.views.length);
        let numViews = this.views.length;
        let textDim = this.measureText(this.name, this.titleFont(), ctx);
        let nodeHeight = Node.NodeHeight * numNodes;
        let fieldHeight = ((Field.fieldHeight * numViews) + textDim.y);
        let fieldWidth = Field.fieldMinWidth; //this.views.reduce((acc, val) => acc + val.dim.x , 0);
        let height = Math.max(nodeHeight, fieldHeight, DefaultDim.y) * TB_Margin * this.scale;
        let width = Math.max(fieldWidth, textDim.x, DefaultDim.x) * LR_Margin * this.scale;
        this.titleDim = textDim;
        return { x: width, y: height };
    }
    adjustParts(ctx) {
        this.dim = this.calcReqDim(ctx);
        this.boxdim = new BoxCoord(this.pos, this.dim);
        this.adjustNodePos(false, this.inputs, this.views, ctx);
        this.adjustNodePos(true, this.outputs, [], ctx);
        this.adjustFieldPos(ctx);
    }
    draw(ctx) {
        super.draw(ctx);
        this.setTitle(ctx);
        this.inputs.forEach(e => e.draw(ctx));
        this.views.forEach(e => e.draw(ctx));
        this.outputs.forEach(e => e.draw(ctx));
        this.fields.forEach(e => e.draw(ctx));
    }
    setPartHosts() {
        for (let i = 0; i < this.inputs.length; ++i) {
            this.inputs[i].host = this;
        }
        for (let i = 0; i < this.outputs.length; ++i) {
            this.outputs[i].host = this;
        }
        for (let i = 0; i < this.views.length; ++i) {
            this.views[i].host = this;
        }
        for (let i = 0; i < this.fields.length; ++i) {
            this.fields[i].host = this;
        }
    }
    constructor(otsClass, name, inputs, outputs, fields, expandable, shape, fill, stroke, strokeWidth = 2, pos = ZeroDim, dim = DefaultDim) {
        super(otsClass, shape, fill, stroke, strokeWidth, pos, dim);
        this.name = name;
        this.inputs = inputs;
        this.outputs = outputs;
        this.fields = fields;
        this.views = [];
        this.allowViews = expandable;
        this.titleDim = { x: 100, y: 30 };
        this.setPartHosts();
    }
    drag(deltaPos, ctx) {
        super.drag(deltaPos, ctx);
        this.adjustParts(ctx);
    }
    setPos(pos, ctx) {
        super.setPos(pos, ctx);
        this.adjustParts(ctx);
    }
    inBound(coord) {
        if (super.inBound(coord) === null) {
            return null;
        }
        for (let i = 0; i < this.inputs.length; ++i) {
            if (this.inputs[i].inBound(coord) !== null)
                return this.inputs[i];
        }
        for (let i = 0; i < this.outputs.length; ++i) {
            if (this.outputs[i].inBound(coord) !== null)
                return this.outputs[i];
        }
        for (let i = 0; i < this.views.length; ++i) {
            if (this.views[i].inBound(coord) !== null)
                return this.views[i];
        }
        for (let i = 0; i < this.fields.length; ++i) {
            if (this.fields[i].inBound(coord) !== null)
                return this.fields[i];
        }
        console.log('Return self');
        return this;
    }
    setScale(scale, ctx) {
        super.setScale(scale, ctx);
        for (let i = 0; i < this.inputs.length; ++i) {
            this.inputs[i].setScale(scale, ctx);
        }
        for (let i = 0; i < this.outputs.length; ++i) {
            this.outputs[i].setScale(scale, ctx);
        }
        for (let i = 0; i < this.views.length; ++i) {
            this.views[i].setScale(scale, ctx);
        }
        for (let i = 0; i < this.fields.length; ++i) {
            this.fields[i].setScale(scale, ctx);
        }
        this.adjustParts(ctx);
    }
}
export class Provider extends Component {
    constructor(name, outputs, fields) {
        super(OTSClass.Provider, name, [], outputs, fields, false, Shapes.Cylinder, Colors.Clear, Colors.White);
    }
}
export class ComponentPart extends Renderable {
    constructor(otsClass, shape, fill, stroke, strokeWidth, pos, dim) {
        super(otsClass, shape, fill, stroke, strokeWidth, pos, dim);
        this.host = null;
    }
    setScale(scale, ctx) {
        super.setScale(scale, ctx);
    }
}
export class Node extends ComponentPart {
    constructor(otsClass, fill, stroke) {
        super(otsClass, Shapes.Oval, fill, stroke, 3, undefined, { x: Node.NodeWidth, y: Node.NodeHeight });
    }
    draw(ctx) {
        super.draw(ctx);
    }
}
Node.NodeHeight = 4;
Node.NodeWidth = 4;
export class Output extends Node {
    constructor() {
        super(OTSClass.Output, Colors.Clear, Colors.Yellow);
    }
}
export class Input extends Node {
    constructor(fill = Colors.Clear, stroke = Colors.Green) {
        super(OTSClass.Input, fill, stroke);
    }
}
export class View extends Input {
    constructor() {
        super(undefined, Colors.Yellow);
    }
}
export class Field extends ComponentPart {
    constructor(type) {
        super(OTSClass.Field, Shapes.Rect, Colors.White, Colors.Black, 2, undefined, { x: Field.fieldMinWidth, y: Field.fieldHeight });
        this.valPair = defaultValue(type);
    }
    draw(ctx) {
        super.draw(ctx);
    }
}
Field.fieldHeight = 18;
Field.fieldMinWidth = 120;
function defaultValue(type) {
    let val = null;
    switch (type) {
        case Types.Signed:
        case Types.Unsigned:
            val = 0;
            break;
        case Types.Decimal:
            val = 0.0;
            break;
        case Types.Bool:
            val = false;
            break;
        case Types.String:
            val = '';
            break;
        case Types.Array:
            val = [];
            break;
        case Types.Map:
            val = {};
            break;
    }
    return { value: val, type: type };
}
function getType(value) {
    if (typeof value === 'number') {
        if (value % Math.trunc(value) != 0) {
            return Types.Decimal;
        }
        return value < 0 ? Types.Signed : Types.Unsigned;
    }
    if (typeof value === 'string') {
        return Types.String;
    }
    if (typeof value === 'boolean') {
        return Types.Bool;
    }
    return value.constructor == Object ? Types.Map : Types.Array;
}
function getValuePairFrom(type, value) {
    let ret = { value: 0, type: Types.Signed };
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
//TODO Util lib
export function deltaPos(pos1, pos2) {
    return { x: pos2.x - pos1.x, y: pos2.y - pos1.y };
}
export function scaleLine(p1, p2, scale) {
    let hyp = Math.sqrt(Math.pow(p2.x - p1.x, 2) + Math.pow(p2.y - p1.y, 2)) * scale;
    let angle = Math.atan2(p2.y - p1.y, p2.x - p1.x);
    let npos = {
        x: Math.cos(angle) * hyp + p1.x,
        y: Math.sin(angle) * hyp + p1.y
    };
    return npos;
}
//# sourceMappingURL=appblueprints.js.map