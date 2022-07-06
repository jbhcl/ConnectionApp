import { User } from "./user";

export class UserParams {
    gender?: string;
    minAge?: number;
    maxAge?: number;
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive';

    constructor() {}
}