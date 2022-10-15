import { useAuth } from "../hooks/useAuth";
import { useState, useEffect } from "react";

export const useFetch = (url) => {
    const [status, setStatus] = useState('idle');
    const [data, setData] = useState([]);

    const { login } = useAuth();

    useEffect(() => {
        if (!url) return;
        const fetchData = async () => {
            setStatus('fetching');
            const response = await fetch(url).catch((err) => {
                login();
              });;
            const data = await response.json();
            setData(data);
            setStatus('fetched');
        };

        fetchData();
    }, [url]);

    return { status, data };
};