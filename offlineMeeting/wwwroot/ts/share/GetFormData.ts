import { ValidateForm } from "./ValidateForm"
export class GetFormData {
    static get = (element: HTMLElement | HTMLFormElement | null, isValidate: boolean = false) => {
        if (!element) {
            throw new Error("is not valid form Element");
        }

        const formElement = element as HTMLFormElement;

        if (isValidate) {
            if (!ValidateForm.validate(formElement)) {
                throw new Error("validation error");
            }
        }
        return new FormData(formElement);
    }

    static getSameClassValueArray = (htmlCollection: HTMLCollectionOf<Element>) => {
        const htmlDataElementArray = Array.from(htmlCollection) as HTMLDataElement[];
        let formValueArray: string[] = [];

        htmlDataElementArray.forEach(htmlDataElement => {
            formValueArray.push(htmlDataElement.value);
        });

        return formValueArray;
    }
}