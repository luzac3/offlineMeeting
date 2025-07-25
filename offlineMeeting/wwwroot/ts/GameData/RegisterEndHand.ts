import { FetchApi } from "@root/share/FetchApi";

export class RegisterEndHand {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    endHandProperty: {
        [key: string]:
            string |
            string[] |
            { [ket: string]: string } |
            { [ket: string]: boolean } |
            { [key: string]: string }[] |
            { [key: string]: boolean }[] |
            null
    } = {
        EventNumber: null,
        HoraKind: null,
        Parent: null,
        RonPointDicList: null,
        PickPointDic: null,
        PaoPointDicList: null,
        TenpaiUsersList: null,
        LeachUsersList: null,
        MyaoUsersList: null,
        VideoIdDic: null,
        VideoNameDic: null,
        ValidVideoDic: null
    };
    
    constructor() {
        this.url = '/GameData/EndHandRegister';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "json";

        this.endHandProperty.EventNumber
            = (<HTMLInputElement>document.getElementById("eventNumber")).value;
    }

    register = async (): Promise<{ [key: string]: string }> => {
        const horaKind = (<HTMLInputElement>document.getElementById("horaKind")).value;

        this.endHandProperty.HoraKind = horaKind;
        this.endHandProperty.Parent = (<HTMLSelectElement>document.getElementById("parent")).value;

        const toUserElementList = document.getElementsByClassName("toUser");
        if (horaKind == "1") {
            // ツモ点数
            const pickPointElementList = document.getElementsByClassName("pickPoint");
            if (pickPointElementList.length) {
                let pickPoinDic: { [key: string]: string } = { UserCd: (<HTMLInputElement>toUserElementList[0]).value };
                let pointList: string[] = []
                Array.from(pickPointElementList).forEach((pickPointElement, index) => {
                    pointList.push((<HTMLInputElement>pickPointElement).value);
                });
                if (pointList.length > 1) {
                    if (parseInt(pointList[0]) > parseInt(pointList[1])) {
                        pickPoinDic["TumoPoint"] = pointList[1];
                        pickPoinDic["ParentTumoPoint"] = pointList[0];
                    } else {
                        pickPoinDic["TumoPoint"] = pointList[0];
                        pickPoinDic["ParentTumoPoint"] = pointList[1];
                    }
                } else {
                    pickPoinDic["TumoPoint"] = pointList[0];
                }
                this.endHandProperty.PickPointDic = pickPoinDic;
            }
        }

        if (horaKind == "2") {
            // ロン点数
            const fromUser = (document.getElementById("fromUser") as HTMLInputElement)?.value ?? null;
            if (toUserElementList.length) {
                let ronDicList: { [key: string]: string }[] = [];
                Array.from(toUserElementList).forEach(toUserElement => {
                    const pointElement = toUserElement.nextElementSibling;
                    ronDicList.push(
                        {
                            UserCd: (<HTMLSelectElement>toUserElement).value,
                            Point: (<HTMLInputElement>pointElement).value,
                            DealInUserCd: fromUser
                        }
                    )
                });
                this.endHandProperty.RonPointDicList = ronDicList;
            }
        }

        // paoのチェック確認
        const paoElement = document.getElementById("pao") as HTMLInputElement;
        if (paoElement.checked) {
            // 包点数　包だけは基本支払い者基準になってる　二重Listを作る
            // Listは[もらった人、あげた人、点数]
            const paoUserElementList = document.getElementsByClassName("paoUser");
            if (paoUserElementList.length) {
                let paoPointDicList: { [key: string]: string }[] = [];
                if (parseInt(horaKind) < 20) {
                    // ダブロン、トリロンでなければToUserもpaoUserも一人　ロンとツモの区別は不要
                    const toUser = (<HTMLSelectElement>document.getElementsByClassName("toUser")[0]).value;
                    const paoUser = (<HTMLSelectElement>paoUserElementList[0]).value;
                    const paoPoint = (<HTMLSelectElement>document.getElementsByClassName("paoPoint")[0]).value;
                    paoPointDicList = [
                        {
                            UserCd: toUser,
                            PaoUserCd: paoUser,
                            PaoPoint: paoPoint
                        }
                    ];
                } else {
                    Array.from(paoUserElementList).forEach((paoUserElement, index) => {
                        if (
                            index > 0 &&
                            (<HTMLInputElement>paoUserElement.previousElementSibling).checked
                        ) {
                            return;
                        }
                        const paoUserToElement = paoUserElement
                            .nextElementSibling!
                            .nextElementSibling!;
                        const paoPointElement = paoUserToElement.nextElementSibling!;
                        paoPointDicList[index] = {
                            UserCd: (<HTMLInputElement>paoUserToElement).value,
                            PaoUserCd: (<HTMLInputElement>paoUserElement).value,
                            PaoPoint: (<HTMLInputElement>paoPointElement).value
                        }
                    });
                }
                this.endHandProperty.PaoPointDicList = paoPointDicList;
            }
        }

        // 立直
        const leachUsersElement = document.getElementsByClassName("leach");
        if (leachUsersElement.length) {
            let leachUsersArray: string[] = [];
            Array.from(leachUsersElement).forEach(element => {
                if ((<HTMLInputElement>element).checked) {
                    const leachCameraNumber = (<HTMLInputElement>element).value;
                    const userCdElement = (<HTMLInputElement>document.getElementById("userCd" + leachCameraNumber));
                    leachUsersArray.push(userCdElement.value);
                }
            });
            if (leachUsersArray.length > 0) {
                this.endHandProperty.LeachUsersList = leachUsersArray;
            }
        }

        // 副露
        const myaoUsersElement = document.getElementsByClassName("myao");
        if (myaoUsersElement.length) {
            let myaoUsersArray: string[] = [];
            Array.from(myaoUsersElement).forEach(element => {
                if ((<HTMLInputElement>element).checked) {
                    const myaoCameraNumber = (<HTMLInputElement>element).value;
                    const userCdElement = (<HTMLInputElement>document.getElementById("userCd" + myaoCameraNumber));
                    myaoUsersArray.push(userCdElement.value);
                }
            });
            if (myaoUsersArray.length > 0) {
                this.endHandProperty.MyaoUsersList = myaoUsersArray;
            }
        }

        // ビデオID
        const videoSelectElement = document.getElementsByClassName("cameraSelector");
        if (videoSelectElement.length) {
            let videoIdDic: { [key: string]: string } = {};
            let videoNameDic: { [key: string]: string } = {};
            let validVideoDic: { [key: string]: boolean } = {};
            Array.from(videoSelectElement).forEach((element, index) => {
                if (index >= 4) {
                    return;
                }
                const userCdElement = element
                    .closest(".videoController")!
                    .querySelector(".userList")! as HTMLSelectElement;
                const selectedIndex = (<HTMLSelectElement>element).selectedIndex;
                const videoCheckerElement = element
                    .closest(".videoController")!
                    .querySelector(".videoChecker")! as HTMLInputElement;
                videoIdDic[userCdElement.value] = (<HTMLSelectElement>element).value;
                videoNameDic[userCdElement.value] = (<HTMLSelectElement>element).options[selectedIndex].text;
                validVideoDic[userCdElement.value] = videoCheckerElement.checked;
            });
            this.endHandProperty.VideoIdDic = videoIdDic;
            this.endHandProperty.VideoNameDic = videoNameDic;
            this.endHandProperty.ValidVideoDic = validVideoDic;
        }

        // 通常流局
        if (horaKind == "30") {
            const tenpaiElement = document.getElementsByClassName("tenpai");
            let tenpaiUsersArray: string[] = [];
            Array.from(tenpaiElement).forEach(element => {
                if ((<HTMLInputElement>element).checked) {
                    tenpaiUsersArray.push((<HTMLInputElement>element).value);
                }
            });
            this.endHandProperty.TenpaiUsersList = tenpaiUsersArray;
        }

        console.log(this.endHandProperty);

        const fetchApi = new FetchApi();

        return await fetchApi.send(
            this.url,
            this.method,
            this.headers,
            this.endHandProperty,
            this.responseKind,
            false
        ).catch(e => {
            throw e;
        });
    }
}