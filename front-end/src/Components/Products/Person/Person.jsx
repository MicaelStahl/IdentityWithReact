import React, { Component } from "react";
import { connect } from "react-redux";

import Loading from "../../UI/Loading";

import * as options from "../../actions/actions/personActions";
import Title from "../../UI/Title";
import GoBackButton from "../../UI/GoBackButton";

class Person extends Component {
  state = {};

  componentDidMount() {
    // ToDo
    this.props.onSiteLoad();
  }

  render() {
    if (this.props.isLoading) {
      return <Loading />;
    } else {
      console.log(this.props.people);
      return (
        <React.Fragment>
          <Title Title="List of all people" />
          <GoBackButton />
          <div>
            <table className="table">
              <caption>List of all people</caption>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Age</th>
                  <th>Email</th>
                  <th>Phonenumber</th>
                  <th>City</th>
                  <th>Postal Code</th>
                  <th>Options</th>
                </tr>
              </thead>
              <tbody>
                {this.props.people.map(person => (
                  <tr key={person.id}>
                    <td>
                      {person.firstName} {person.lastName}
                    </td>
                    <td>{person.age}</td>
                    <td>{person.email}</td>
                    <td>{person.phoneNumber}</td>
                    <td>{person.city}</td>
                    <td>{person.postalCode}</td>
                    <td>options</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </React.Fragment>
      );
    }
  }
}

const mapStateToProps = state => {
  return {
    people: state.person.people,
    isLoading: state.options.isLoading
  };
};

const mapDispatchToProps = dispatch => {
  return {
    onSiteLoad: () => dispatch(options.FindAllAsync())
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Person);
