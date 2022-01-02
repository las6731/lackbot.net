export class JsonParser {
    parse(text: string): any {
        text = text.replace(regex, s => `"${s}"`); // convert bigints to strings before parsing, so they don't get corrupted
        if (text == '') return null; // it doesn't like empty strings for some reason
        console.log(text);
        return JSON.parse(text, customReviver);
    }
}

const regex = /(?<=:)[0-9]{16,}/g; // string of numbers at least 16 characters long, preceded by a :
const fullRegex = /^[0-9]{16,}$/g; // string of numbers at least 16 characters long, with nothing else before/after

const customReviver = (key: string, value: any) => {
    if (typeof value == 'string') {
        if (value.match(fullRegex)) return BigInt(value); // cast stringified bigints to bigint
    }
    return value;
};
