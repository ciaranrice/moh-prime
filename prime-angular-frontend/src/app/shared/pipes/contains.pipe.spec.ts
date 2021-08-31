import { ContainsPipe } from './contains.pipe';

describe('ContainsPipe', () => {
  const line = 'Hello World';
  let pipe: ContainsPipe;
  let word;
  beforeEach(() => {
    pipe = new ContainsPipe(),
    word = '';
  });

  it('create an instance', () => expect(pipe).toBeTruthy());

  describe('testing with string includes', () => {
    it('should return true for all tests', () => {
      word = 'Hello';
      expect(pipe.transform(word, line)).toBeTruthy();

      word = 'World';
      expect(pipe.transform(word, line)).toBeTruthy();
    });

    it('should return false', () => {
      word = 'Goodbye';
      expect(pipe.transform(word, line)).toBeFalsy();
    });
  });

  describe('testing with string startsWith', () => {
    it('should return true', () => () => {
      word = 'Hello';
      expect(pipe.transform(word, line, 'startsWith')).toBeTruthy();
    });

    it('should return false', () => () => {
      word = 'World';
      expect(pipe.transform(word, line, 'startsWith')).toBeFalsy();
    });
  });

  describe('testing with string ends', () => {
    it('should return true', () => () => {
      word = 'World';
      expect(pipe.transform(word, line, 'endsWith')).toBeTruthy();
    });

    it('should return false', () => () => {
      word = 'Hello';
      expect(pipe.transform(word, line, 'endsWith')).toBeFalsy();
    });
  });

  describe('testing with null values', () => {
    it('should return false with 1 null value for line', () => () => {
      expect(pipe.transform(word, null)).toBeFalsy();
    });

    it('should return false with 1 null value for word', () => () => {
      expect(pipe.transform(null, line)).toBeFalsy();
    });

    it('should return false with 2 null values', () => () => {
      expect(pipe.transform(null, null)).toBeFalsy();
    });
  });
});