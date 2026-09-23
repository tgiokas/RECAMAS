import { Container } from '@/components/Containers/Container/Container';
import { linkedProfiles, profileCases } from '../dummyData';
import styles from './CasesTab.module.css';

export default function CasesTab() {
  return (
    <>
      <Container>
        <h3 className={styles.heading}>Cases</h3>
        <ul className={styles.list}>
          {profileCases.map((item) => (
            <li key={item.id} className={styles.item}>
              <div className={styles.info}>
                <span className={styles.title}>{item.id}</span>
                <span className={styles.meta}>Type: {item.type}</span>
                <span className={styles.meta}>Stage: {item.stage}</span>
                <span className={styles.meta}>Status: {item.status}</span>
                <span className={styles.meta}>Officer: {item.assignedOfficer}</span>
              </div>
              <button type="button" className={styles.link} onClick={() => undefined}>
                View case
              </button>
            </li>
          ))}
        </ul>
      </Container>

      <Container>
        <h3 className={styles.heading}>Linked Profiles</h3>
        <ul className={styles.list}>
          {linkedProfiles.map((item) => (
            <li key={item.id} className={styles.item}>
              <div className={styles.info}>
                <span className={styles.title}>{item.name}</span>
                <span className={styles.meta}>Relationship: {item.relationship}</span>
                <span className={styles.meta}>Profile ID: {item.id}</span>
                <span className={styles.meta}>ARC: {item.arc}</span>
                <span className={styles.meta}>Nationality: {item.nationality}</span>
              </div>
              <a className={styles.link} href="#" onClick={(event) => event.preventDefault()}>
                View profile
              </a>
            </li>
          ))}
        </ul>
      </Container>
    </>
  );
}
