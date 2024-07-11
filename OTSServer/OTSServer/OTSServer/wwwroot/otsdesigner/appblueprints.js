export var Shapes;
(function (Shapes) {
    Shapes[Shapes["Oval"] = 0] = "Oval";
    Shapes[Shapes["Rect"] = 1] = "Rect";
    Shapes[Shapes["Cylinder"] = 2] = "Cylinder";
    Shapes[Shapes["RoundRect"] = 3] = "RoundRect";
    Shapes[Shapes["Curve"] = 4] = "Curve";
    Shapes[Shapes["Line"] = 5] = "Line";
})(Shapes || (Shapes = {}));
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
var Types;
(function (Types) {
    Types[Types["String"] = 0] = "String";
    Types[Types["Signed"] = 1] = "Signed";
    Types[Types["Unsigned"] = 2] = "Unsigned";
    Types[Types["Bool"] = 3] = "Bool";
    Types[Types["Decimal"] = 4] = "Decimal";
    Types[Types["Map"] = 5] = "Map";
    Types[Types["Array"] = 6] = "Array";
})(Types || (Types = {}));
const defaultDim = { x: 50, y: 50 };
const zeroDim = { x: 0, y: 0 };
export class OTSObject {
    inBound(coord) {
        return ((coord.x > (this.pos.x - this.dim.x / 2)) &&
            (coord.x < (this.pos.x + this.dim.x / 2)) &&
            (coord.y > (this.pos.y - this.dim.y / 2)) &&
            (coord.y < (this.pos.y + this.dim.y / 2)));
    }
    setPos(pos) {
        this.pos = pos;
    }
    constructor(pos = zeroDim, dim = defaultDim) {
        this.pos = pos;
        this.dim = dim;
    }
}
export class Renderable extends OTSObject {
    constructor(shape, fill, stroke, strokeWidth = 2, pos = zeroDim, dim = defaultDim) {
        super(pos, dim);
        this.shape = shape;
        this.fill = fill;
        this.stroke = stroke;
        this.strokeWidth = strokeWidth;
        this.activeFill = fill;
        this.activeStroke = stroke;
    }
    draw(ctx) {
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
            ctx.stroke();
        }
        else if (this.shape == Shapes.Line) {
        }
        else if (this.shape == Shapes.Curve) {
        }
        if (this.activeStroke != Colors.Clear) {
            ctx.stroke();
        }
        if (this.activeFill != Colors.Clear) {
            ctx.fill();
        }
    }
}
export class Draggable extends Renderable {
    constructor(shape, fill, stroke, strokeWidth = 2, pos = zeroDim, dim = defaultDim) {
        super(shape, fill, stroke, strokeWidth, pos, dim);
    }
    dragTo(delta) {
        this.pos.x += delta.x;
        this.pos.y += delta.y;
    }
}
export class Component extends Renderable {
    adjustNodePos(isLeft, nodes1, nodes2) {
        let x = isLeft ? this.pos.x - this.dim.x / 2 : this.pos.x + this.dim.x / 2;
        let numNodes = nodes1.length + nodes2.length;
        let dy = this.dim.y / (numNodes + 1);
        let ty = this.pos.y - this.dim.y / 2;
        for (let i = 0; i < nodes1.length; ++i) {
            nodes1[i].setPos({ x: x, y: ty + dy * (i + 1) });
        }
        for (let i = 0; i < nodes2.length; ++i) {
            nodes1[i].setPos({ x: x, y: ty + dy * (i + nodes1.length + 1) });
        }
    }
    adjustFieldPos() {
    }
    adjustParts() {
        this.adjustNodePos(false, this.inputs, this.views);
        this.adjustNodePos(true, this.outputs, []);
        this.adjustFieldPos();
    }
    draw(ctx) {
        super.draw(ctx);
        this.inputs.forEach(e => e.draw(ctx));
        this.views.forEach(e => e.draw(ctx));
        this.outputs.forEach(e => e.draw(ctx));
        this.fields.forEach(e => e.draw(ctx));
    }
    constructor(name, inputs, outputs, fields, expandable, shape, fill, stroke, strokeWidth = 2, pos = zeroDim, dim = defaultDim) {
        super(shape, fill, stroke, strokeWidth, pos, dim);
        this.name = name;
        this.inputs = inputs;
        this.outputs = outputs;
        this.fields = fields = [];
        this.views = [];
        this.allowViews = expandable;
        this.adjustParts();
    }
    setPos(pos) {
        super.setPos(pos);
        this.adjustParts();
    }
}
export class Provider extends Component {
    constructor(name, outputs, fields) {
        super(name, [], outputs, fields, false, Shapes.Cylinder, Colors.Clear, Colors.White);
    }
}
export class Node extends Renderable {
    constructor(fill, stroke) {
        super(Shapes.Oval, fill, stroke, 3, undefined, { x: 4, y: 4 });
    }
    draw(ctx) {
        super.draw(ctx);
    }
}
export class Output extends Node {
    constructor() {
        super(Colors.Clear, Colors.Yellow);
    }
}
export class Input extends Node {
    constructor(fill = Colors.Clear, stroke = Colors.Green) {
        super(fill, stroke);
    }
}
export class View extends Input {
    constructor() {
        super(undefined, Colors.Yellow);
    }
}
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
export class Field extends Renderable {
    constructor(type) {
        super(Shapes.Rect, Colors.White, Colors.Black, 2);
        this.valPair = defaultValue(type);
    }
}
//# sourceMappingURL=appblueprints.js.map