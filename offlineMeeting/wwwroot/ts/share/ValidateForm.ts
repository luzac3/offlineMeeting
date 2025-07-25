export class ValidateForm
{
    static validate = (
        formElement: HTMLFormElement
    ) => {
        // validation警告を削除
        $(".validationError").remove();
        let validateCorrect: boolean = true;

        const formData = new FormData(formElement);

        const htmlFormControlsCollectionArray = Array.from(formElement.elements);
        htmlFormControlsCollectionArray.forEach((element: Element) => {
            const name = element.getAttribute("name");
            if (!name) {
                return false;
            }
            
            if (
                element.getAttribute("data-require") == "1" &&
                !formData.get(name)
            ) {
                $(element).after(
                    "<p style='background-color:red' class='validationError'>！必須項目です</p>"
                );
                validateCorrect = false;
            }
        });
        return validateCorrect;
    }
}