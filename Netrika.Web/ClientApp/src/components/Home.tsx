import React, { Component, useState } from 'react';
import { Button, Input } from 'reactstrap';
import { loadMedicalOrganizations } from '../api/medical-organizations';

export const Home = () => {

  const [searchTerm, setSearchTerm] = useState('');

  const handleSearch = async () => {
    const result = await loadMedicalOrganizations(searchTerm);
    console.log(result);
  }

  return (
    <>
      <Input
        type="text"
        value={searchTerm}
        onChange={e => setSearchTerm(e.target.value)}
      />
      <Button onClick={handleSearch}>Search</Button>
    </>
  );
}
