import { SetOutSameClassSelect } from "@root/share/SetOutSameClassSelect"
import { SetEventListner } from "@root/share/SetEventListner"

export class ModifiedUserSelect {
    static setAvoidDuplication = () => {
        const setOutSameClassSelect = new SetOutSameClassSelect();
        const userClass = document.getElementsByClassName("userList") as HTMLCollectionOf<HTMLSelectElement>;
        const directionClass = document.getElementsByClassName("directionList") as HTMLCollectionOf<HTMLSelectElement>;
        setOutSameClassSelect.setSelectorEvent(userClass);
        setOutSameClassSelect.setSelectorEvent(directionClass);
    }

    static hangingVideoSetter = (parentElementName: string) => {
        const userClass = document.getElementsByClassName("userList") as HTMLCollectionOf<HTMLSelectElement>;
        Array.from(userClass).forEach((userElement, index) => {
            const parentElement = userElement.closest(parentElementName) as HTMLElement;
            SetEventListner.setEvent(
                parentElement,
                "change",
                "#userCd"+index,
                (event) => {
                    const targetDirection = (<HTMLSelectElement>event.target)
                        .closest(".videoController")!
                        .querySelector(".directionList");
                    if ((<HTMLSelectElement>event.target).value == "99") {
                        (<HTMLSelectElement>targetDirection).value = "99";
                        (<HTMLSelectElement>targetDirection).disabled = true;
                        // changeイベントを発火させる
                        (<HTMLSelectElement>targetDirection).dispatchEvent(new Event("change"));
                    } else {
                        (<HTMLSelectElement>targetDirection).disabled = false;
                    }
                }
            )
        });
    }
}