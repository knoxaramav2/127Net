import { Provider, Output, Field, Types } from "./appblueprints.js";
export function toOtsJObj(raw) {
    console.log('CONVERSION');
    console.log(raw);
    console.log(`CONVERTING ${raw.name}`);
    var outputs = [
        new Output(), new Output(), new Output(), new Output(),
    ];
    var fields = [
        new Field(Types.String), new Field(Types.String), new Field(Types.String)
    ];
    var component = new Provider(raw.name, outputs, fields);
    return component;
}
//# sourceMappingURL=otsclassconverter.js.map