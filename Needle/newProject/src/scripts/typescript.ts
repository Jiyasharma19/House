import { Behaviour, serializable } from "@needle-tools/engine";

// Documentation → https://docs.needle.tools/scripting

export class typescript extends Behaviour {

    @serializable()
    myStringField: string = "Hello World";
    
    start() {
    }
}