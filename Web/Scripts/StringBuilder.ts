// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

namespace DigitalBeacon {
    export class StringBuilder {
        _parts: string[];

        cat(str: string): StringBuilder {
            this._parts.push(str);
            return this;
        }

        build(): string {
            return this._parts.join("");
        }
    }
}
