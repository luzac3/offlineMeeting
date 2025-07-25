/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./wwwroot/ts/EventList/ControlModal.ts":
/*!**********************************************!*\
  !*** ./wwwroot/ts/EventList/ControlModal.ts ***!
  \**********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   ControlModal: () => (/* binding */ ControlModal)
/* harmony export */ });
class ControlModal {
    constructor() {
        this.setControl = () => {
            $(".closeModal").on("click", () => {
                this.hidden();
            });
            $("#modalWrapper").on("click", (event) => {
                event.target.closest("#modalWindow") ?? this.hidden();
            });
            $("#newEvent").on("click", () => {
                this.show();
            });
        };
        this.show = () => {
            $("#modalWrapper").removeClass("hidden");
        };
        this.hidden = () => {
            $("#modalWrapper").addClass("hidden");
        };
    }
}


/***/ }),

/***/ "./wwwroot/ts/EventList/ControlObjectHandler.ts":
/*!******************************************************!*\
  !*** ./wwwroot/ts/EventList/ControlObjectHandler.ts ***!
  \******************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   ControlObjectHandler: () => (/* binding */ ControlObjectHandler)
/* harmony export */ });
/* harmony import */ var _root_share_ControlObject__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/ControlObject */ "./wwwroot/ts/share/ControlObject.ts");

class ControlObjectHandler {
    constructor() {
        this.setControlObjectHandler = () => {
            $("#addUser").on("click", () => {
                const lastUserCdObject = this.getLastUserCdObject(document.getElementsByClassName("UsersCdWrapper"));
                _root_share_ControlObject__WEBPACK_IMPORTED_MODULE_0__.ControlObject.copyObject(lastUserCdObject);
            });
            $("#delUser").on("click", () => {
                const lastUserCdObject = this.getLastUserCdObject(document.getElementsByClassName("UsersCdWrapper"));
                _root_share_ControlObject__WEBPACK_IMPORTED_MODULE_0__.ControlObject.elaseObject(lastUserCdObject);
            });
        };
        this.getLastUserCdObject = (htmlCollectionOf) => {
            const userCdObjects = Array.from(htmlCollectionOf);
            const lastUserCdObject = userCdObjects.pop();
            if (!lastUserCdObject) {
                throw new Error("un collect userCdObject");
            }
            return lastUserCdObject;
        };
    }
}


/***/ }),

/***/ "./wwwroot/ts/EventList/ManagementStreaming.ts":
/*!*****************************************************!*\
  !*** ./wwwroot/ts/EventList/ManagementStreaming.ts ***!
  \*****************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   ManagementStreaming: () => (/* binding */ ManagementStreaming)
/* harmony export */ });
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");

class ManagementStreaming {
    constructor() {
        this.managementStreaming = async (formDataEntryValue) => {
            const fetchApi = new _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__.FetchApi();
            let url = this.url;
            url += "?";
            Object.keys(formDataEntryValue).forEach(key => {
                url += key + "=" + formDataEntryValue[key] + "&";
            });
            url = url.slice(0, -1);
            fetchApi.send(url, this.method, this.headers, null, this.responseKind).then(() => {
                location.href = url;
            }).catch(e => {
                console.log(e);
            });
        };
        this.management = () => {
            $(".management").on("click", (event) => {
                const eventNumber = event.target.parentElement?.getAttribute("data-eventNumber");
                if (!eventNumber) {
                    console.log("event Number error");
                    return;
                }
                this.managementStreaming({
                    EventNumber: eventNumber
                });
            });
        };
        this.url = '/ManagementStreaming/Index';
        this.method = 'GET';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
}


/***/ }),

/***/ "./wwwroot/ts/EventList/RegisterNewEvent.ts":
/*!**************************************************!*\
  !*** ./wwwroot/ts/EventList/RegisterNewEvent.ts ***!
  \**************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   RegisterNewEvent: () => (/* binding */ RegisterNewEvent)
/* harmony export */ });
/* harmony import */ var _root_share_GetFormData__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/GetFormData */ "./wwwroot/ts/share/GetFormData.ts");
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");


class RegisterNewEvent {
    constructor() {
        this.register = async (formDataEntryValue) => {
            const fetchApi = new _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_1__.FetchApi();
            fetchApi.send(this.url, this.method, this.headers, formDataEntryValue, this.responseKind).then((data) => {
                if (data == "1") {
                    window.alert("register Collect");
                    location.reload();
                }
                else {
                    throw new Error(data);
                }
            }).catch(e => {
                console.log(e);
            });
        };
        this.setRegisterEventListner = () => {
            $("#registerButton").on("click", (event) => {
                try {
                    const userCdObjectValueArray = _root_share_GetFormData__WEBPACK_IMPORTED_MODULE_0__.GetFormData.getSameClassValueArray(document.getElementsByClassName("UsersCd"));
                    const formData = _root_share_GetFormData__WEBPACK_IMPORTED_MODULE_0__.GetFormData.get(document.getElementById("registerForm"), true);
                    formData.append("UsersCd", userCdObjectValueArray.join());
                    const formDataEntryValue = Object.fromEntries(formData.entries());
                    this.register(formDataEntryValue);
                }
                catch (e) {
                    // fixme
                    console.log(e);
                }
            });
        };
        this.url = '/EventList/RegisterEvent';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
}


/***/ }),

/***/ "./wwwroot/ts/EventList/StartEvent.ts":
/*!********************************************!*\
  !*** ./wwwroot/ts/EventList/StartEvent.ts ***!
  \********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   StartEvent: () => (/* binding */ StartEvent)
/* harmony export */ });
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");

class StartEvent {
    constructor() {
        this.startEvent = async (formDataEntryValue) => {
            const fetchApi = new _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__.FetchApi();
            let url = this.url;
            url += "?";
            Object.keys(formDataEntryValue).forEach(key => {
                url += key + "=" + formDataEntryValue[key] + "&";
            });
            url = url.slice(0, -1);
            fetchApi.send(url, this.method, this.headers, null, this.responseKind).then(() => {
                location.href = url;
            }).catch(e => {
                console.log(e);
            });
        };
        this.start = () => {
            $(".start").on("click", (event) => {
                const eventNumber = event.target.parentElement?.getAttribute("data-eventNumber");
                if (!eventNumber) {
                    console.log("event Number error");
                    return;
                }
                this.startEvent({
                    EventNumber: eventNumber
                });
            });
        };
        this.url = '/Event/Index';
        this.method = 'GET';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
}


/***/ }),

/***/ "./wwwroot/ts/EventList/Streaming.ts":
/*!*******************************************!*\
  !*** ./wwwroot/ts/EventList/Streaming.ts ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   Streaming: () => (/* binding */ Streaming)
/* harmony export */ });
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");

class Streaming {
    constructor() {
        this.Streaming = async (formDataEntryValue) => {
            const fetchApi = new _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__.FetchApi();
            let url = this.url;
            url += "?";
            Object.keys(formDataEntryValue).forEach(key => {
                url += key + "=" + formDataEntryValue[key] + "&";
            });
            url = url.slice(0, -1);
            fetchApi.send(url, this.method, this.headers, null, this.responseKind).then(() => {
                location.href = url;
            }).catch(e => {
                console.log(e);
            });
        };
        this.streaming = () => {
            $(".streaming").on("click", (event) => {
                const eventNumber = event.target.parentElement?.getAttribute("data-eventNumber");
                if (!eventNumber) {
                    console.log("event Number error");
                    return;
                }
                this.Streaming({
                    EventNumber: eventNumber
                });
            });
        };
        this.url = '/Streaming/Index';
        this.method = 'GET';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/ControlObject.ts":
/*!*******************************************!*\
  !*** ./wwwroot/ts/share/ControlObject.ts ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   ControlObject: () => (/* binding */ ControlObject)
/* harmony export */ });
class ControlObject {
}
ControlObject.copyObject = (object) => {
    const cloneObject = object.cloneNode(true);
    object.after(cloneObject);
};
ControlObject.elaseObject = (object) => {
    object.remove();
};


/***/ }),

/***/ "./wwwroot/ts/share/FetchApi.ts":
/*!**************************************!*\
  !*** ./wwwroot/ts/share/FetchApi.ts ***!
  \**************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   FetchApi: () => (/* binding */ FetchApi)
/* harmony export */ });
class FetchApi {
    constructor() {
        this.send = async (url, method, headers, body, 
        //body: any,
        responseKind = "json", needResponseData = true) => {
            let request;
            if (method == "GET") {
                request = new Request(url, {
                    method: method,
                    headers: headers
                });
            }
            else {
                request = new Request(url, {
                    method: method,
                    headers: headers,
                    body: JSON.stringify(body)
                });
            }
            const response = await fetch(request).catch(e => { throw e; });
            if (!response.ok) {
                throw new Error(response.statusText);
            }
            if (!needResponseData) {
                return;
            }
            switch (responseKind) {
                case "json":
                    return await this.fetchResonse(response.json()).catch(e => { throw e; });
                case "text":
                    return await this.fetchResonse(response.text()).catch(e => { throw e; });
            }
        };
    }
    async fetchResonse(response) {
        return await response.then(data => {
            return data;
        })
            .catch(error => {
            // エラー処理
            console.log(error);
            throw error;
        });
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/GetFormData.ts":
/*!*****************************************!*\
  !*** ./wwwroot/ts/share/GetFormData.ts ***!
  \*****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   GetFormData: () => (/* binding */ GetFormData)
/* harmony export */ });
/* harmony import */ var _ValidateForm__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./ValidateForm */ "./wwwroot/ts/share/ValidateForm.ts");

class GetFormData {
}
GetFormData.get = (element, isValidate = false) => {
    if (!element) {
        throw new Error("is not valid form Element");
    }
    const formElement = element;
    if (isValidate) {
        if (!_ValidateForm__WEBPACK_IMPORTED_MODULE_0__.ValidateForm.validate(formElement)) {
            throw new Error("validation error");
        }
    }
    return new FormData(formElement);
};
GetFormData.getSameClassValueArray = (htmlCollection) => {
    const htmlDataElementArray = Array.from(htmlCollection);
    let formValueArray = [];
    htmlDataElementArray.forEach(htmlDataElement => {
        formValueArray.push(htmlDataElement.value);
    });
    return formValueArray;
};


/***/ }),

/***/ "./wwwroot/ts/share/ValidateForm.ts":
/*!******************************************!*\
  !*** ./wwwroot/ts/share/ValidateForm.ts ***!
  \******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   ValidateForm: () => (/* binding */ ValidateForm)
/* harmony export */ });
class ValidateForm {
}
ValidateForm.validate = (formElement) => {
    // validation警告を削除
    $(".validationError").remove();
    let validateCorrect = true;
    const formData = new FormData(formElement);
    const htmlFormControlsCollectionArray = Array.from(formElement.elements);
    htmlFormControlsCollectionArray.forEach((element) => {
        const name = element.getAttribute("name");
        if (!name) {
            return false;
        }
        if (element.getAttribute("data-require") == "1" &&
            !formData.get(name)) {
            $(element).after("<p style='background-color:red' class='validationError'>！必須項目です</p>");
            validateCorrect = false;
        }
    });
    return validateCorrect;
};


/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
// This entry need to be wrapped in an IIFE because it need to be isolated against other modules in the chunk.
(() => {
/*!***************************************!*\
  !*** ./wwwroot/ts/EventList/Index.ts ***!
  \***************************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _ControlModal__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./ControlModal */ "./wwwroot/ts/EventList/ControlModal.ts");
/* harmony import */ var _RegisterNewEvent__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./RegisterNewEvent */ "./wwwroot/ts/EventList/RegisterNewEvent.ts");
/* harmony import */ var _ControlObjectHandler__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./ControlObjectHandler */ "./wwwroot/ts/EventList/ControlObjectHandler.ts");
/* harmony import */ var _StartEvent__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./StartEvent */ "./wwwroot/ts/EventList/StartEvent.ts");
/* harmony import */ var _ManagementStreaming__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ./ManagementStreaming */ "./wwwroot/ts/EventList/ManagementStreaming.ts");
/* harmony import */ var _Streaming__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./Streaming */ "./wwwroot/ts/EventList/Streaming.ts");






(() => {
    const controlModal = new _ControlModal__WEBPACK_IMPORTED_MODULE_0__.ControlModal();
    const registerNewEvent = new _RegisterNewEvent__WEBPACK_IMPORTED_MODULE_1__.RegisterNewEvent();
    const controlObjectHandler = new _ControlObjectHandler__WEBPACK_IMPORTED_MODULE_2__.ControlObjectHandler();
    const startEvent = new _StartEvent__WEBPACK_IMPORTED_MODULE_3__.StartEvent();
    const managementStreaming = new _ManagementStreaming__WEBPACK_IMPORTED_MODULE_4__.ManagementStreaming();
    const streaming = new _Streaming__WEBPACK_IMPORTED_MODULE_5__.Streaming();
    controlModal.setControl();
    registerNewEvent.setRegisterEventListner();
    controlObjectHandler.setControlObjectHandler();
    startEvent.start();
    managementStreaming.management();
    streaming.streaming();
})();

})();

/******/ })()
;
//# sourceMappingURL=EventList.js.map