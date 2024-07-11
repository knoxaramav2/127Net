import { 
    Component,
    Provider,

    Output,
    Input,
    View
    } from "./appblueprints.js";

export function toOtsJObj(raw: any) {
    console.log('CONVERSION')
    console.log(raw)

    console.log(`CONVERTING ${raw.name}`)

    var outputs = [new Output()];
    var component = new Provider(raw.name, outputs, []);

    return component;
}