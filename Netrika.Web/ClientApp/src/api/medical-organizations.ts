export interface MedicalOrganization {
    id: string,
    name: string
}

const customFetchWithResponse = async <TBody extends any>(url: string, method: string, body?: TBody) => {
    const response = await fetch(url, {
        method: method,
        body: body ? JSON.stringify(body) : undefined,
        // headers: {
        //     ['content-type']: 'text/plain'
        // }
    });
    if (!response.ok) {
        throw new Error(response.statusText);
    }
    return Promise.resolve(await response.json());
}

export const loadMedicalOrganizations = (searchTerm: string): Promise<MedicalOrganization[]> => {
    return customFetchWithResponse<MedicalOrganization[]>('MedicalOrganizations', "GET");
}