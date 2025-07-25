export class GetCameraInformation {
    static cameraContainer: { [key: string]: string }[] = [];

    static setDevices = async () => {
        const devices = (await navigator.mediaDevices.enumerateDevices());
        GetCameraInformation.cameraContainer = [];
        devices.forEach(device => {
            if (device.kind !== "videoinput") {
                return;
            }
            GetCameraInformation.cameraContainer.push({
                deviceId: device.deviceId,
                label: device.label
            });
        });
        return GetCameraInformation.cameraContainer;
    }

    static createCameraSelectorObject = (selectObject: Element) => {
        if (selectObject.childElementCount > 0) {
            Array.from(selectObject.children).forEach((element: Element) => {
                element.remove();
            });
        }
        GetCameraInformation.cameraContainer.forEach((camera) => {
            const cameraOptionElement = document.createElement("option");
            cameraOptionElement.value = camera.deviceId;
            cameraOptionElement.text = camera.label;
            selectObject.appendChild(cameraOptionElement);
        });
    }
}