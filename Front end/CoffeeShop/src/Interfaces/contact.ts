import { UserContact } from "./userContact";

export interface Contact {
    id : string;
    customer:UserContact;
    adminName:string; 
    sendDate: Date;
    subject: string;
    description: string;
    problemName: string;
    response: string;
    status: string;
}