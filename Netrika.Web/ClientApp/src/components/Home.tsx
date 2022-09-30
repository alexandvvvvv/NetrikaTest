import { useState } from 'react';
import { Button, Input } from 'reactstrap';
import { getMedicalOrganization, loadMedicalOrganizations, MedicalOrganization } from '../api/medical-organizations';
import InfiniteScroll from 'react-infinite-scroller';

const pageSize = 20;

export const Home = () => {

  const [searchTerm, setSearchTerm] = useState('');
  const [organizations, setOrganizations] = useState<MedicalOrganization[]>([]);
  const [hasMore, setHasMore] = useState<boolean>(false);

  const handleSearchByTerm = async () => {
    const result = await loadMedicalOrganizations(searchTerm, 0, pageSize);
    setOrganizations(result);
    setHasMore(true); //todo return this value from API 
  }
  const handleSearchById = async () => {
    const result = await getMedicalOrganization(searchTerm);
    setOrganizations([result]);
  }
  const handleLoadMore = async () => {
    const result = await loadMedicalOrganizations(searchTerm, organizations.length, pageSize);
    setOrganizations(x => [...x, ...result]);
    setHasMore(!!result.length); //todo return this value from API 
  }

  return (
    <>
      <Input
        type="text"
        value={searchTerm}
        onChange={e => setSearchTerm(e.target.value)}
      />
      <Button onClick={handleSearchByTerm}>Search by term</Button>
      <Button onClick={handleSearchById}>Search by ID</Button>

      <InfiniteScroll loadMore={handleLoadMore} hasMore={hasMore}>
        {
          organizations.map((x, i) => (
            <div key={i}>
              {x.name} <small>({x.id})</small>
            </div>
          ))
        }
      </InfiniteScroll>
    </>
  );
}
