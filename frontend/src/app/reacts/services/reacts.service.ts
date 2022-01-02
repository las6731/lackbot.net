import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AutoReact } from '../models/autoreact.model';

@Injectable({
    providedIn: 'root'
})
export class ReactsService {

    $reacts: Subject<AutoReact[]>;

    constructor(private httpClient: HttpClient) {
        this.$reacts = new Subject();
    }

    public getReacts(): void {
        this.httpClient.get(`${environment.API_URL}/auto-reacts`).subscribe((res: any) => {
            let reacts = res as any[];  
            let results: AutoReact[] = [];

            reacts.forEach(react => {
                results.push(AutoReact.Parse(react));
            });

            this.$reacts.next(results);
        });
    }

    public addAutoReact(react: AutoReact): void {
        let command = react as any;
        command.author = react.author?.toString();
        if (command.author == '') delete command.author;
        this.httpClient.post(`${environment.API_URL}/auto-reacts`, command)
            .subscribe(() => this.getReacts());
    }

    public replaceReact(react: AutoReact): void {
        let command = react as any;
        command.author = react.author?.toString();
        if (command.author == '') delete command.author;
        this.httpClient.post(`${environment.API_URL}/auto-reacts/${react.id}`, command)
            .subscribe(() => this.getReacts());
    }

    public deleteReact(id: string): void {
        this.httpClient.delete(`${environment.API_URL}/auto-reacts/${id}`)
            .subscribe(() => this.getReacts());
    }
}
