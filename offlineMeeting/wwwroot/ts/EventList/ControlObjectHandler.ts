import { ControlObject } from "@root/share/ControlObject";

export class ControlObjectHandler {
    setControlObjectHandler = () => {
        $("#addUser").on("click", () => {
            const lastUserCdObject = this.getLastUserCdObject(document.getElementsByClassName("UsersCdWrapper"))
            ControlObject.copyObject(lastUserCdObject);
        });
        $("#delUser").on("click", () => {
            const lastUserCdObject = this.getLastUserCdObject(document.getElementsByClassName("UsersCdWrapper"))
            ControlObject.elaseObject(lastUserCdObject);
        });
    }

    private getLastUserCdObject = (htmlCollectionOf: HTMLCollectionOf<Element>) => {
        const userCdObjects = Array.from(htmlCollectionOf);
        const lastUserCdObject = userCdObjects.pop();
        if (!lastUserCdObject) {
            throw new Error("un collect userCdObject");
        }
        return lastUserCdObject as HTMLElement;
    }
}