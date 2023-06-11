import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/internal/Observable";
import { EmailSettings } from "src/models/emailSettings";


@Injectable({
    providedIn: 'root'
})
export class EmailSettingsService{
    
    constructor(private http:HttpClient){}

    getGeneralProperties(){
        
        return this.http.get('/api/emailsettings/general');
    }

    setNewEmailSettings(data: EmailSettings): Observable<any>{
        return this.http.post('/api/emailsettings/setgeneral', data);
    }
}