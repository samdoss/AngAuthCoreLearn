import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'viewpost',
    templateUrl: './viewpost.component.html',

})
export class ViewPostComponent {
    public postdata: PostData;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        let postId = localStorage.getItem("postId");
        console.log(postId, "postId");
        http.get(baseUrl + 'api/WebBoardPost/Get?postId=' + postId).subscribe(result => {
            console.log('result  v  result '  , result);
            this.postdata = result.json() as PostData;
        }, error => console.error(error));
    }
}


interface PostData {
    postId: number;
    postSubject: string;
    postDetail: string;
    postTimeStamp: Date;
}