export class AutoReact {

    public id: string | undefined;
    public description: string;
    public phrase: string;
    public emoji: string;
    public author: bigint | undefined;
    public type: AutoReactType;

    public constructor(description: string, phrase: string, emoji: string) {
        this.description = description;
        this.phrase = phrase;
        this.emoji = emoji;
        this.type = AutoReactType.Naive;
    }

    public static Parse(apiResponse: any): AutoReact {
        let react = new AutoReact(apiResponse.phrase, apiResponse.phrase, apiResponse.emoji);
        react.id = apiResponse.id;

        let type: string = apiResponse.$type;
        if (type.includes(AutoReactType.Author)) {
            react.type = AutoReactType.Author;
            react.author = BigInt(apiResponse.author);
        } else if (type.includes(AutoReactType.Strong)) {
            react.type = AutoReactType.Strong;
        } else {
            react.type = AutoReactType.Naive;
        }

        return react;
    }
}

export enum AutoReactType {
    Naive = "Naive",
    Strong = "Strong",
    Author = "Author"
}
