import { Provider, Output } from "./appblueprints.js";
export function toOtsJObj(raw) {
    console.log('CONVERSION');
    console.log(raw);
    console.log(`CONVERTING ${raw.name}`);
    var outputs = [new Output()];
    var component = new Provider(raw.name, outputs, []);
    return component;
}
//# sourceMappingURL=otsclassconverter.js.map