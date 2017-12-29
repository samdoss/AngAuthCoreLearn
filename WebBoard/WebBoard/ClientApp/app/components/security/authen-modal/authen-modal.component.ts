import { Component, OnInit, ViewChild,Input } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap';

import {AuthenModel} from './model/v1/authen-model'; 
/* Import service */

@Component({
    selector: 'authen-modal',
    templateUrl: './authen-modal.component.html',
    styleUrls: ['./authen-modal.conponent.scss']
})

export class AuthenModalComponent {
    constructor(){
        
    }
    AuthenModel : AuthenModel = new AuthenModel();
    @Input('getAuthenModel') getAuthenModel : AuthenModel;
    @ViewChild('AfterAuthenModal') public AfterAuthenModal: ModalDirective;
    
    show() {
        this.AfterAuthenModal.show();
    }
}
