import { Container } from 'reactstrap';

export const Layout = ({children}: { children: JSX.Element }) => {

    return (
      <div>
        <Container>
          {children}
        </Container>
      </div>
    );
}
