import { GetFormData } from "@root/share/GetFormData";
import { FetchApi } from "@root/share/FetchApi";

export class RegisterNewEvent {
    url: string;
    method: string;
    headers: { [key: string]: string };
    responseKind: string;
    constructor() {
        this.url = '/EventList/RegisterEvent';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
    register = async (formDataEntryValue: { [key: string]: FormDataEntryValue }) => {
        const fetchApi = new FetchApi();
        fetchApi.send(
            this.url,
            this.method,
            this.headers,
            formDataEntryValue,
            this.responseKind
        ).then((data) => {
            if (data == "1") {
                window.alert("register Collect");
                location.reload();
            } else {
                throw new Error(data);
            }
        }).catch(e => {
            console.log(e);
        });
    }

    setRegisterEventListner = () => {
        $("#registerButton").on("click", (event) => {
            try {
                const userCdObjectValueArray = GetFormData.getSameClassValueArray(document.getElementsByClassName("UsersCd"));
                const formData = GetFormData.get(document.getElementById("registerForm"), true);

                formData.append("UsersCd", userCdObjectValueArray.join());
                const formDataEntryValue = Object.fromEntries(formData.entries());

                this.register(formDataEntryValue);
            } catch (e) {
                // fixme
                console.log(e);
            }
        });
    }
}