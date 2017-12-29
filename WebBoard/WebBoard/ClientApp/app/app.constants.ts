import { Injectable } from '@angular/core';

import { UserModel, } from './components/models/user-model';

@Injectable()
export class Configuration {

    public WebBoardClientId: string = "03162ea57ec34b578f1190a9d783d9df";
    public apiWebBoardURL: string = 'http://localhost:57798/';
}

@Injectable()
export class Constants {
    public today = new Date().toJSON().split('T')[0];

    public userData: UserModel = <UserModel>{};

    constructor() {
    }
}